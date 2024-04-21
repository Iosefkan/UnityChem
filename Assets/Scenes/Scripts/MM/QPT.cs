using System;
using Types;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Numerics;
using System.Linq;

namespace Program
{
    class QPT
    {
        public QPT(){}

        public void init(InitData initData, bool isAloneQ = false)
        {
            init(initData.data, initData.sect, initData.cyl, initData.dop, isAloneQ);
        }

        public void init(DATA_ data, SECT[] sect, CYLINDER[] cyl, DOP_DATA dop, bool isAloneQ = false)
        {
            for (int i = 0; i < sect.Length; ++i)
            {
                sect[i].Order = i + 1;
            }

            table = "";

            DataRec = data;
            sect.CopyTo(SCT, 0);
            cyl.CopyTo(CYL, 0);
            DOP = dop;
            if (isAloneQ)
            {
                DOP.n_Alfa = 0;
                DOP.Alfa_max = 0;
                DOP.Alfa_min = 0;
            }

            Res.MQ = new double[DOP.n_Alfa + 1];
            Res.MT = new double[DOP.n_Alfa + 1];
            Res.MP = new double[DOP.n_Alfa + 1];

            nSect = DataRec.nS_1+DataRec.nS_2;

            //{ Расстановка нарезанных секций червяка }
            for (int j = 0; j < nSect; j++)
            {
                int i = -1;
                do
                {                    
                    i++;
                    if (SCT[i].Order == j+1)
                    {
                        SCT[28] = SCT[j];
                        SCT[j] = SCT[i];
                        SCT[i] = SCT[28];
                    }
                } 
                while (!(SCT[i].Order == j+1 || i+1 == nSect));
            }

            //{ Начальные присвоения }
            L = 0; //{Длина рабочей части червяка}
            Lambda[0] = 0;
            for (int i = 0; i < nSect; i++)
            {
                SCT[i].D_st = SCT[i].D_st / 1000;
                SCT[i].D_fin = SCT[i].D_fin / 1000;
                SCT[i].L_sect = SCT[i].L_sect / 1000;
                SCT[i].H_st = SCT[i].H_st / 1000;
                SCT[i].H_fin = SCT[i].H_fin / 1000;
                if (SCT[i].S_Type == 1)
                {
                    SCT[i].step_ = SCT[i].step_ / 1000;
                    SCT[i].e_st = SCT[i].e_st / 1000;
                    SCT[i].e_fin = SCT[i].e_fin / 1000;
                    SCT[i].delta = SCT[i].delta / 1000;
                    SCT[i].d_Fi = SCT[i].d_Fi / 180 * Math.PI;
                    SCT[i].W_a = SCT[i].W_a / 1000;
                }
                L = L + SCT[i].L_sect;
                Lambda[i+1] = L;
            }

            //for (iS_korp = 0; iS_korp <= DataRec.nS_korp; iS_korp++)
            //{
            //    DataRec.Lam_kor[iS_korp] = DataRec.Lam_kor[iS_korp] / 1000; //{ Назначение чисел циклов интегрирования по секциям червяка }
            //}
            
            nLm = DataRec.n_Integr;
            j = 0;
            for (int i = 0; i < nSect; i++)
            {
                SCT[i].n_cykle = (int)Math.Round(nLm * SCT[i].L_sect / L);
                if (SCT[i].n_cykle == 0)
                    SCT[i].n_cykle = 1;
                j = j + SCT[i].n_cykle;
            }
            if (j > nLm)
                nLm = j;
            
            n_Eta = 60; //{ Число шагов интегрирования по глубине канала }
            for (int i = 0; i <= n_Eta; i++)
            {
                Eta[i] = (double)i / n_Eta;
            }

            Mu0 = DataRec.Mu0_ * 1000;
            T0 =  DataRec.T0_;
            b =   DataRec.b_;
            m =   DataRec.m_;
            n =   DataRec.N_;
            Ro =  DataRec.Ro_;
            T =   DataRec.T_St;
            if (DataRec.Var_Tscr == 1)
            {
                T_screw = DataRec.T_scr;
            }
            else
            {
                T_screw = DataRec.T_St;
            }
            Al_korp = DataRec.Al_kor;
            Al_screw = DataRec.Al_scr;
            a_T = DataRec.a_T_;
            Lam_T = DataRec.Lam_T_;
            T_Start = DataRec.T_St;
            C_S = Lam_T / a_T / Ro;

            c = 1 / m + 1;

            //{Табличное представление определяющего расчетного уравнения
            //               теории плоских потоков}
            s = 10;
            mb[0] = 0;
            db = 0.8 / s;
            for (i = 1; i <= 4 * s; i++)
            {
                mb[i] = mb[i - 1] + db;
                mf[i] = Math.Abs((mb[i] - 1) / (mb[i] + 1));
                mf[i] = Math.Exp(c * Math.Log(mf[i])) - 1;
                mf[i] = Math.Abs(c * (mf[i] + 2) / mf[i] + 1 / mb[i]);
                if (i == s)
                    db = 5.0 / s;
                if (i == 2 * s)
                    db = 2.0 / m / s;
                if (i == 3 * s)
                    db = (6.0 / m - 5.8) / s;
            }
            
            for(i_Alfa = 0; i_Alfa <= DOP.n_Alfa; i_Alfa++)
            {
                //{ Начало циклов по ALPHA }
                TETA = 1 - DOP.Alfa_min - (DOP.Alfa_max - DOP.Alfa_min) * i_Alfa / (double)DOP.n_Alfa;
                T = T_Start;
                iSect = 0;
                Lm = 0;
                p = 0;
                H = Ht();
                st = stL();
                e = et();
                Mu = Mu0 * Math.Exp(-b * (T - T0));
                k = -1;
                Volume = 0;

                //Если Q не известно
                if (!isAloneQ)
                {
                    // {Выбор сечения червяка для определения Q машины в целом}
                    for (i = 0; i < nSect; i++)
                    {
                        if (SCT[i].S_Type == 1)
                        {
                            D = SCT[i].D_st;
                            pd = Math.PI * D;
                            u = pd * n / 60.0;
                            fi_ = Math.Atan(SCT[i].step_ / pd);
                            uz_ = u * Math.Cos(fi_);
                            W_st = pd * Math.Sin(fi_) / SCT[i].n_Line - SCT[i].e_st * Math.Cos(fi_);
                            S_st = SCT[i].H_st * W_st * SCT[i].n_Line;
                            W_fin = pd * Math.Sin(fi_) / SCT[i].n_Line - SCT[i].e_fin * Math.Cos(fi_);
                            S_Fin = SCT[i].H_fin * W_fin * SCT[i].n_Line;
                            if (i == 0 || S_st < SQ)
                            {
                                SQ = S_st;
                                Q_M = (1 - TETA) * uz_ * W_st * SCT[i].H_st * SCT[i].n_Line / 2;
                            }
                            if (S_Fin < SQ)
                            {
                                SQ = S_Fin;
                                Q_M = (1 - TETA) * uz_ * W_fin * SCT[i].H_fin * SCT[i].n_Line / 2;
                            }
                        }
                    }
                }
                else //Если Q известно
                {
                    Q_M = DOP.Q;
                }

                Res.MQ[i_Alfa] = Q_M * 1E+6;

                nPrint = (int)Math.Round(nLm / 15.0);
                ic = nPrint;
                T = DataRec.T_PL_;
                Start_PL = true;
                pr_T = false;

                X_PL_LIST.Add(new List<double>());

                for (iSect = 0; iSect < nSect; iSect++) //{Циклы по секциям}
                {
                    if (SCT[iSect].S_Type == 1)
                    {
                        D = SCT[iSect].D_st;
                        pd = Math.PI * D;
                        u = pd * n / 60.0; //{ Секции с нарезкой }
                        dLm = SCT[iSect].L_sect / SCT[iSect].n_cykle;
                        nL = SCT[iSect].n_Line;
                        st = stL();
                        fi = Math.Atan(st / Math.PI / D);
                        cs = Math.Cos(fi);
                        sn = Math.Sin(fi);
                        dz = dLm / sn;
                        a_ = pd / cs;
                        g_ = uX * SCT[iSect].delta / 2.0;
                        Q_delta0 = a_ * g_;
                        Q_delta = Q_delta0;
                        Q_a = 0;
                        for (i = 0; i <= SCT[iSect].n_cykle; i++) //{ Циклы вдоль секции с нарезой }
                        {
                            if (iSect == 0 && i == 0)
                            {
                                H = Ht();
                                e = et();
                                W = pd * sn / nL - e * cs;
                                H0 = H;
                                W0 = W;
                                X_PL = W;
                                X_OTN[0] = 1;
                                H_pr = Ht();
                                Q = 0;
                            }
                            if (i > 0) //{ Поиск текущей секции корпуса }
                            {
                                DLINA = 0;
                                iCYL = -1;
                                do
                                {
                                    iCYL++;
                                    DLINA += CYL[iCYL].L_sec;
                                } 
                                while (!(DLINA >= Lm * 1000 || iCYL+1 == DataRec.nS_korp));

                                if (CYL[iCYL].Var_T == 1) //{ Параметры теплообмена с корпусом }
                                {
                                    T_korp = CYL[iCYL].T_W_k_;
                                }
                                else
                                {
                                    AL_W_k = CYL[iCYL].Al_W_k_;
                                    Del_k = CYL[iCYL].Del_k_ / 1000;
                                    T_W_k = CYL[iCYL].T_W_k_;
                                    Q_W_k = CYL[iCYL].Q_W_k_;
                                    pr = AL_W_k * Del_k / DataRec.Lam_k + AL_W_k / Al_korp;
                                    TB_k = (T + pr * T_W_k) / (pr + 1);
                                    q_Wk = AL_W_k * (TB_k - T_W_k);
                                    T_korp = T - AL_W_k / Al_korp * (TB_k - T_W_k);
                                }
                                
                                if (DataRec.Var_Tscr != 1) //{ Параметры теплообмена с червяком }
                                {
                                    pr = DataRec.Al_W_s * DataRec.Del_s / DataRec.Lam_s + DataRec.Al_W_s / Al_screw;
                                    TB_s = (T + pr * DataRec.T_W_s) / (pr + 1);
                                    q_Ws = DataRec.Al_W_s * (TB_s - DataRec.T_W_s);
                                    T_screw = T - DataRec.Al_W_s / Al_screw * (TB_s - DataRec.T_W_s);
                                }

                                Lm += dLm;
                                H = Ht();
                                e = et();
                                Mu = Mu0 * Math.Exp(-b * (T - T0));
                                W = pd * sn / nL - e * cs;
                                uZ = u * cs;
                                uX = u * sn;
                                Q_B = uZ * W * H / 2;
                                MuEf_Fix = Mu * Math.Exp((m - 1) * Math.Log(u / H));
                                if (X_PL == 0)
                                {
                                    dQ_delta = 0.5 * Q_delta0;
                                    tQ_delta = 0.0001 * Q_B;
                                    if (dQ_delta < 0.01 * Q_B)
                                    {
                                        dQ_delta = 0.01 * Q_B;
                                    }
                                    Mu_delta = Mu0 * Math.Exp(-b * (T_korp - T0)) * Math.Exp((m - 1) * Math.Log(u / SCT[iSect].delta));
                                    b_ = st * SCT[iSect].delta * SCT[iSect].delta * SCT[iSect].delta / 
                                            (12 * Mu_delta * nL * sn * (e * cs + SCT[iSect].delta));
                                    Q = (Q_M + Q_delta + Q_a) / nL;
                                }
                                else
                                {
                                    Q_memory = Q_M / nL;
                                    if (iSect == 0 && i == 1)
                                    {
                                        G_k = Q_memory * Ro;
                                        F1dXdZ = dXdZ();
                                        v_SZ = G_k / W / H / DataRec.Ro_gran_;
                                    }
                                }

                                dHdZ = (H - H_pr) / dz;
                                H_pr = H;
                                Mu_a = Mu * Math.Exp((m - 1) * Math.Log(u / H));
                                if ((X_PL == 0 && Math.Abs(1 - Q / Q_B) < 0.2)||  (X_PL > 0))  /*{ при безнапорном течении }*/
                                {
                                    func = 0.001;
                                    Chorda(Q_delta0, dQ_delta, tQ_delta, ref Q_delta, ref func, find_Q_delta);
                                }
                                else //{ при напорном течении }
                                {
                                    test = 0;
                                    Metka:
                                    if (X_PL == 0)
                                    {
                                        Q = (Q_M + Q_delta + Q_a) / nL;
                                    }
                                    else
                                    {
                                        Q_memory = Q_M / nL;
                                        Q_b_ = uZ * (W - X_PL) * H / 2;
                                        if (iSect == 0 && i == 1)
                                        {
                                            G_k = Q_memory * Ro;
                                            F1dXdZ = dXdZ();
                                            v_SZ = G_k / W / H / DataRec.Ro_gran_;
                                        }
                                    }

                                    TETA = 1 - Q / Q_B;
                                    AL_Z = Q / Q_B;
                                    Q_B_X = H * uX / 2;
                                    Q_ut = Q_delta / a_;
                                    AL_X = Q_ut / Q_B_X;
                                    if (i == 1)
                                    {
                                        MuEf_a();
                                    }

                                    itera = 0;
                                    do
                                    {
                                        itera++;
                                        Del = 0;
                                        Int_i();
                                        Roots();
                                        MuEff();
                                        MinMax(0, n_Eta, Mu_ef, ref fMin, ref fMax);
                                        MinMax(0, n_Eta, Mu_eff, ref fMin, ref ffMax);
                                        if (ffMax > fMax)
                                        {
                                            fMax = ffMax;
                                        }
                                        for (j = 0; j <= n_Eta; j++)
                                        {
                                            Del += Math.Pow((Mu_ef[j] - Mu_eff[j]) / fMax, 2);
                                        }
                                        Del = Math.Sqrt(Del / n_Eta);
                                        IterEnd = (itera == 40) || (Del < 0.002);
                                        for (j = 0; j <= n_Eta; j++)
                                        {
                                            Mu_ef[j] = Mu_eff[j];
                                        }
                                    } while (!IterEnd);
                                    fd_fp();
                                    Qj_delta = Q_delta;
                                    b_a = st * H * H * H / (12 * Mu_a * nL * sn * (e * cs + H));
                                    sign_dp = (dp_dz == 0) ? 0 : dp_dz / Math.Abs(dp_dz);
                                    if (dp_dz != 0)
                                    {
                                        dp_dz = (nL * Q_B * fd - Q_M - a_ * g_) / 
                                                (nL * (Q_B - Q) * fp / dp_dz + a_ * b_ + SCT[iSect].W_a * b_a) * sign_dp;
                                    }

                                    Q_delta = a_ * (b_ * dp_dz + g_);
                                    Q_a = SCT[iSect].W_a * b_a * dp_dz;
                                    test++;
                                    if (Math.Abs((Q_delta - Qj_delta) / Q_B) > 0.001 && test < 15)
                                    {
                                        goto Metka;
                                    }
                                }
                                
                                tkz = H * dp_dz * (1 - Eta_Z);
                                tkx = H * dp_dx * (1 - Eta_X);
                                if (X_PL == 0)
                                {
                                    p += dp_dz * dz;
                                    dn = (uZ * tkz + uX * tkx) * W;
                                    T += (dn + W * (Al_korp * (T_korp - T) + Al_screw * (T_screw - T)) - dp_dz * Q) / Q * a_T / Lam_T * dz;
                                }
                                else
                                {
                                    if (X_PL > 0.05 * W && !Start_PL)
                                    {
                                        Q = Q_memory - v_SZ * H * X_PL * DataRec.Ro_gran_ / Ro;
                                        p += dp_dz * dz; //{ Приращение удельного давления }
                                        dn = (uZ*tkz+uX*tkx)*(W-X_PL); //{Расчёт текущей}
                                        dT = (dn + (W - X_PL) * (Al_korp * (T_korp - T) + //{температуры}
                                            Al_screw * (T_screw - T)) - dp_dz * Q) / //{расплава в зоне}
                                            Q * a_T / Lam_T * dz; //{плавления}
                                        if (pr_T)
                                        {
                                            dT -= dQ_rasp * (p / Q * //{червячной}
                                                a_T / Lam_T + (T - DataRec.T_PL_) / Q);
                                        }
                                        T += dT; //{ машины}
                                    }
                                    F2dXdZ = dXdZ();
                                    dX_PL = (F1dXdZ + F2dXdZ) / 2 * dz;
                                    F1dXdZ = F2dXdZ;
                                    X_PL -= dX_PL;
                                    dQ_rasp = v_SZ * H * dX_PL * DataRec.Ro_gran_ / Ro;
                                    pr_T = true;
                                    if (X_PL <= 0.05 * W)
                                    {
                                        X_PL = 0;
                                    }
                                    if (X_PL <= 0.95 * W)
                                    {
                                        Start_PL = false;
                                    }
                                }
                                Volume += H * W * dz * nL;
                            }
                            
                            if (ic == nPrint || i == SCT[iSect].n_cykle)
                            {
                                ic = 0;

                                if (i == 0)
                                {
                                    table += "Плавное удельное давление p, температура расплавленного полимера T, геометрия шнека\n" +
                                              "граничное напряжение сдвига, ширина твердой фазы и элементарный вклад\n" +
                                              "к температуре компонентов мощности на Q_machine = " + Q_M.ToString() + " см^3/сек\n" +
                                              String.Format("{0,-12}{1,-12}{2,-12}{3,-12}{4,-12}{5,-12}{6,-12}{7,-12}{8,-12}{9,-12}{10,-12}{11,-12}\n",
                                              "Lambda", "p", "T", "H", "W", "t", "Tau_Z", "X", "N_uz", "N_ux", "N_AL", "N_dp");
                                }

                                table +=  String.Format("|{0,10:d3}|{1,10:f3}|{2,10:f3}|{3,10:f3}|{4,10:f3}|{5,10:d3}|", 
                                                         (int)Math.Round(Lm * 1000), p * 1E-6, T, H * 1000, W * 1000, (int)Math.Round(st * 1000));
                                if (i == 0)
                                {
                                    table += '\n';
                                }
                                else
                                {
                                    table += String.Format("{0,10:d3}|{1,10:f3}|", (int)Math.Round(tkz / 1000), X_PL * 1000);
                                }
                                if (!Start_PL)
                                {
                                    table += String.Format("{0,10:f3}|{1,10:f3}|{2,10:f3}|{3,10:f3}|\n", 
                                                        uZ * tkz * W / Q * a_T / Lam_T * dz, 
                                                        uX * tkx * W / Q * a_T / Lam_T * dz,
                                                        W * (Al_korp * (T_korp - T) + Al_screw * (T_screw - T)) / Q * a_T / Lam_T * dz, 
                                                        dp_dz * Q / Q * a_T / Lam_T * dz);
                                }

                                k++;
                                ZM[k] = Lm;
                                TM[k] = T;
                                PM[k] = p * 1E-6;
                                X_OTN[k] = X_PL / W;
                                X_PL_LIST.Last().Add(X_PL);
                            }
                            ic++;
                        }
                    } //{ конец цикла по i для секции с нарезкой }
                    else
                    {
                        dLm = SCT[iSect].L_sect / SCT[iSect].n_cykle; //{ Г л а д к а я  с е к ц и я }
                        for (i = 0; i <= SCT[iSect].n_cykle; i++) //{ Циклы вдоль гладкой секции }
                        {
                            if (i > 0) //{ Поиск текущей секции корпуса }
                            {
                                DLINA = 0;
                                iCYL = -1;
                                do
                                {
                                    iCYL++;
                                    DLINA += CYL[iCYL].L_sec;
                                } 
                                while (!(DLINA >= Lm * 1000 || iCYL == DataRec.nS_korp)); //{ Параметры теплообмена с корпусом }

                                if (CYL[iCYL].Var_T == 1)
                                {
                                    T_korp = CYL[iCYL].T_W_k_;
                                }
                                else
                                {
                                    AL_W_k = CYL[iCYL].Al_W_k_;
                                    Del_k = CYL[iCYL].Del_k_ / 1000;
                                    T_W_k = CYL[iCYL].T_W_k_;
                                    Q_W_k = CYL[iCYL].Q_W_k_;
                                    pr = AL_W_k * Del_k / DataRec.Lam_k + AL_W_k / Al_korp;
                                    TB_k = (T + pr * T_W_k) / (pr + 1);
                                    q_Wk = AL_W_k * (TB_k - T_W_k);
                                    T_korp = T - AL_W_k / Al_korp * (TB_k - T_W_k);
                                }
                                
                                //{ Параметры теплообмена с червяком }
                                if (DataRec.Var_Tscr != 1)
                                {
                                    pr = DataRec.Al_W_s * DataRec.Del_s / DataRec.Lam_s + 
                                        DataRec.Al_W_s / Al_screw;
                                    TB_s = (T + pr * DataRec.T_W_s) / (pr + 1);
                                    q_Ws = DataRec.Al_W_s * (TB_s - DataRec.T_W_s);
                                    T_screw = T - DataRec.Al_W_s / Al_screw * (TB_s - DataRec.T_W_s);
                                }
                                D = SCT[iSect].D_st + i * (SCT[iSect].D_fin - SCT[iSect].D_st) / SCT[iSect].n_cykle;
                                Lm += dLm;
                                dz = dLm;
                                Mu = Mu0 * Math.Exp(-b * (T - T0));
                                rH = D / 2;
                                rB = rH - Ht();
                                MuEf_Fix = Mu * Math.Exp((m - 1) * Math.Log(u / (rH - rB)));
                                for (j = 0; j <= n_Eta; j++)
                                {
                                    r[j] = rB + j * (rH - rB) / n_Eta;
                                }
                                
                                if (i == 1)
                                {
                                    MuEf_a();
                                }
                                itera = 0;
                                do
                                {
                                    itera++;
                                    Del = 0;
                                    Int_5();
                                    Roots_g();
                                    MuEff_g();
                                    MinMax(0, n_Eta, Mu_ef, ref fMin, ref fMax);
                                    MinMax(0, n_Eta, Mu_eff, ref fMin, ref ffMax);
                                    if (ffMax > fMax)
                                    {
                                        fMax = ffMax;
                                    }
                                    for (j = 0; j <= n_Eta; j++)
                                    {
                                        Del += Math.Pow((Mu_ef[j] - Mu_eff[j]) / fMax, 2);
                                    }
                                    Del = Math.Sqrt(Del / n_Eta);
                                    IterEnd = (itera == 40) || (Del < 0.002);
                                    for (j = 0; j <= n_Eta; j++)
                                    {
                                        Mu_ef[j] = Mu_eff[j];
                                    }
                                }
                                while (!IterEnd);

                                Tau_rt_B = Tau_rt_H * rH / rB;
                                dp_dz = 2 * (Tau_rz_H * rH - Tau_rz_B * rB) / (rH * rH - rB * rB);
                                p += dp_dz * dz;
                                T += (2 * Math.PI * rH * (u * Tau_rt_H + Al_korp * (T_korp - T) + //T_korp
                                    rB / rH * Al_screw * (T_screw - T)) - dp_dz * Q_M) / Q_M * a_T / Lam_T * dz;
                                Volume += Math.PI * (rH * rH - rB * rB) * dz;
                            }

                            if (ic == nPrint || i == SCT[iSect].n_cykle)
                            {
                                ic = 0;
                                table += String.Format("|{0,10:d3}|{1,10:f3}|{2,10:f3}|{3,10:f3}|",
                                                        (int)Math.Round(Lm * 1000), p * 1E-6, T, (rH - rB) * 1000);
                                if (X_PL > 0)
                                {
                                    table += String.Format("       Пробка гранул недоплавлена на {0,10:f3}%", X_PL / W * 100);
                                    //X_PL = 0;
                                }
                                table += '\n';
                                k++;
                                ZM[k] = Lm;
                                TM[k] = T;
                                PM[k] = p * 1E-6;
                                X_OTN[k] = X_PL / W;
                                X_PL_LIST.Last().Add(X_PL);
                            }

                            ic++;
                        } //{ конец цикла по i для гладкой секции }
                    } 
                }//{ cycle by iSect }

                //Данные для графика
                PM_LIST.Add(PM);
                ZM_LIST.Add(ZM);
                TM_LIST.Add(TM);
                X_OTN_LIST.Add(X_OTN);

                PZ.Add(new List<Vector2>());
                TZ.Add(new List<Vector2>());
                XZ.Add(new List<Vector2>());
                ZXPT.Add(new List<List<double>>());
                for (int i = 0; i < k; ++i)
                {
                    PZ.Last().Add(new Vector2((float)(ZM[i] * 1e3), (float)PM[i]));
                    TZ.Last().Add(new Vector2((float)(ZM[i] * 1e3), (float)TM[i]));
                    XZ.Last().Add(new Vector2((float)(ZM[i] * 1e3), (float)X_PL_LIST.Last()[i] * 1000));
                    ZXPT.Last().Add(new List<double>
                    {
                        XZ.Last().Last().X,
                        XZ.Last().Last().Y,
                        PZ.Last().Last().Y,
                        TZ.Last().Last().Y,
                    }) ;
                }

                T_Min = fMin;
                T_Max = fMax;

                //Результат
                Res.MT[i_Alfa] = T;
                Res.MP[i_Alfa] = p * 1E-6;

            }//{ Цикла по TETA };

            Res.a = a_T;
            Res.Lam = Lam_T;
            Res.n_Q = DOP.n_Alfa;

            Res.Mu0 = Mu0;
            Res.b = b;
            Res.n = m;
            Res.T0 = T0;
        }

        // Свободная глубина винтового канала
        public double Ht()
        {
            return SCT[iSect].H_st + (SCT[iSect].H_fin - SCT[iSect].H_st) / SCT[iSect].L_sect * (Lm - Lambda[iSect]);
        }

        // Плавный шаг винтового полета
        public double stL()
        {
            return SCT[iSect].step_;
        }

        // Плавная толщина витка винта
        public double et()
        {
            return SCT[iSect].e_st + (SCT[iSect].e_fin - SCT[iSect].e_st) / SCT[iSect].L_sect * (Lm - Lambda[iSect]);
        }

        double dXdZ()
        {            
            return X_PL / H * dHdZ + W0 * H0 / H / G_k * 
                    Math.Sqrt(Ro * uX / 2 * Lam_T * (T_korp - DataRec.T_PL_) /
                    (C_S * (DataRec.T_PL_ - DataRec.T_St) + DataRec.K_PL_) * X_PL);
        }

        public double find_Q_delta()
        {            
            Q = (Q_M + Q_delta + Q_a) / nL;
            TETA = 1 - Q / Q_B;
            AL_Z = Q / Q_B;
            Q_B_X = H * uX / 2;
            Q_ut = Q_delta / a_;
            AL_X = Q_ut / Q_B_X;
                        
            if (i == 1)
            {
                MuEf_a();
            }

            itera = 0;
            do
            {
                itera++;
                Del = 0;

                Int_i();
                Roots();
                MuEff();
                MinMax(0, n_Eta, Mu_ef, ref fMin, ref fMax);
                MinMax(0, n_Eta, Mu_eff, ref fMin, ref ffMax);
                
                if (ffMax > fMax)
                {
                    fMax = ffMax;
                }

                for (j = 0; j <= n_Eta; j++)
                {
                    Del += Math.Pow((Mu_ef[j] - Mu_eff[j]) / fMax, 2);
                }

                Del = Math.Sqrt(Del / n_Eta);
                IterEnd = (itera == 40) || (Del < 0.002);
                for (j = 0; j <= n_Eta; j++)
                {
                    Mu_ef[j] = Mu_eff[j];
                }
            } 
            while (!IterEnd);

            Qj_delta = Q_delta;
            Q_delta = a_ * (b_ * dp_dz + g_);
            b_a = st * H * H * H / (12 * Mu_a * nL * sn * (e * cs + H));
            Q_a = SCT[iSect].W_a * b_a * dp_dz;

            return (Q_delta - Qj_delta) / Q_B;
        }
        public bool flag12345 = false;
        // Начальное приближение в виде Mu_ef=const
        void MuEf_a()
        {
            for (int i = 0; i <= n_Eta; i++)
            {
                Mu_ef[i] = 1;
            }
        }

        // Промежуточные определенные интегралы
        public void Int_i()
        {
            double f1 = 0, f2 = 0, f3 = 0, f4 = 0, f5 = 0, f6 = 0, d = 0;
            int i;
            I0 = 0;
            I1 = 0;
            I2 = 0;
            d = 1.0 / n_Eta / 2.0;

            for (i = 0; i <= n_Eta; i++)
            {
                f2 = 1 / Mu_ef[i] / MuEf_Fix;
                f4 = f2 * Eta[i];
                f6 = f4 * Eta[i];
                if (i > 0)
                {
                    I0 = I0 + (f1 + f2) * d;
                    I1 = I1 + (f3 + f4) * d;
                    I2 = I2 + (f5 + f6) * d;
                }
                f1 = f2;
                f3 = f4;
                f5 = f6;
            }
        }
        
        void Roots() //{ Четыре корня расчетных уравнений }
        {
            Eta_Z = (I1 - I2 - AL_Z * I1 / 2) / (I0 - I1 - AL_Z * I0 / 2);                
            dp_dz = uZ / (H * H * (I1 - Eta_Z * I0));
            Eta_X = (I1 - I2 - AL_X * I1 / 2) / (I0 - I1 - AL_X * I0 / 2);
            dp_dx = uX / (H * H * (I1 - Eta_X * I0));
        }
        
        // Новое приближение для эффективной вязкости в секции с нарезкой
        void MuEff()
        {
            int i;
            double a;
            for (i = 0; i <= n_Eta; i++)
            {
                a = Mu_ef[i] * MuEf_Fix;
                Gamma_XX[i] = H / a * dp_dx * (Eta[i] - Eta_X);
                Gamma_ZZ[i] = H / a * dp_dz * (Eta[i] - Eta_Z);
                Mu_eff[i] = Math.Sqrt(Math.Pow(Gamma_XX[i], 2) + Math.Pow(Gamma_ZZ[i], 2));
                Mu_eff[i] = Mu * Math.Exp((m - 1) * Math.Log(Mu_eff[i])) / MuEf_Fix;
            }
        }
        
        //{Минимум и максимум элементов массива}
        public void MinMax(int iMin, int iMax, double[] X, ref double xMin, ref double xMax)
        {            
            for (int i = iMin; i <= iMax; i++)
            {
                if (i == iMin || X[i] < xMin)
                {
                    xMin = X[i];
                }
                
                if (i == iMin || X[i] > xMax)
                {
                    xMax = X[i];
                }
            }
        }

        void Chorda(double x0, double dx, double tx, ref double x, ref double y, Func<double> F)
        {            
            double x1, x2, y1, y2, ty;
            byte k;
            bool v;
            ty = y;            
            x = x0;            
            y = F();
            if (Math.Abs(y) < ty) return;
            x1 = x;
            y1 = y;
            x = x0 + dx;
            y = F();
            if (Math.Abs(y) < ty) return;
            x2 = x;
            y2 = y;
            k = 2;
            
            do
            {
                if (transit) x = (x1 + x2) / 2; // DIV 2 method
                else x = x1 - (x2 - x1) / (y2 - y1) * y1; // CHORDA method
                y = F();
                k++;
                if (Math.Abs(y) < ty) return;
                if (transit || (y1 / Math.Abs(y1) * y2 / Math.Abs(y2) < 0))
                {
                    if (y / Math.Abs(y) * y1 / Math.Abs(y1) < 0)
                    {
                        x2 = x;
                        y2 = y;
                    }
                    else
                    {
                        x1 = x;
                        y1 = y;
                    }
                }
                else if (Math.Abs(y1) < Math.Abs(y2))
                {
                    x2 = x;
                    y2 = y;
                }
                else
                {
                    x1 = x;
                    y1 = y;
                }
                v = (k > 30) && !transit;
            } 
            while (!(((Math.Abs(x2 - x1) < tx) && transit) || (Math.Abs(y) <= ty) || v));
            if (v) transit = true;
        }

        void fd_fp() //{Коэффициенты стенок канала}
        {
            double n, k;
            ff = H / W;
            fd = 0;
            fp = 0;
            for (j = 1; j <= 20; j++)
            {
                n = Math.PI * (2 * j - 1);
                k = n * n;
                fd = fd + th(n / 2 * ff) / (n * k);
                fp = fp + th(n / 2 / ff) / (k * k * n);
            }
            fd = 16 / ff * fd;
            fp = 1 - 192 * ff * fp;
        }
        
        public static double th(double x) // Тангенс гиперболический
        {
            double a; 
            if (x > 40)
            {
                a = 0;
            }
            else
            {
                a = Math.Exp(-2 * x);
            }
            return (1 - a) / (1 + a);
        }

        // Промежуточные определенные интегралы для гладкой секции
        public void Int_5()
        {
            double f1 = 0, f2 = 0, f3 = 0, f4 = 0, f5 = 0, f6 = 0, 
                    f7 = 0, f8 = 0, f9 = 0, f10 = 0, f11 = 0, f12 = 0, 
                    d = 0, a = 0, b = 0;
            int i;
            I1 = 0;
            I2 = 0;
            I3 = 0;
            I4 = 0;
            I5 = 0;
            d = (rH - rB) / n_Eta / 2;
            for (i = 0; i <= n_Eta; i++)
            {
                b = rH * rH - r[i] * r[i];
                f2 = rB / r[i] * b / (rH * rH - rB * rB);
                f1 = rH / r[i] * (r[i] * r[i] - rB * rB) / (rH * rH - rB * rB);
                a = Mu_ef[i] * MuEf_Fix;
                f4 = rH / (r[i] * a);
                f6 = f1 / a;
                f8 = f2 / a;
                f10 = b * f1 / a;
                f12 = b * f2 / a;
                if (i > 0)
                {
                    I5 = I5 + (f3 + f4) * d;
                    I1 = I1 + (f5 + f6) * d;
                    I2 = I2 + (f7 + f8) * d;
                    I3 = I3 + (f9 + f10) * d;
                    I4 = I4 + (f11 + f12) * d;
                }
                f3 = f4;
                f5 = f6;
                f7 = f8;
                f9 = f10;
                f11 = f12;
            }
        }
        
        public void Roots_g()
        {
            double a;
            a = Math.PI * (I1 * I4 - I2 * I3);
            Tau_rz_H = -Q_M * I2 / a;
            Tau_rz_B = Q_M * I1 / a;
            Tau_rt_H = u / I5;
        }

        // Новое приближение для эффективной вязкости в гладкой секции
        void MuEff_g()
        {
            int i;
            double a, b, c, d, f1, f2;            
            for (i = 0; i <= n_Eta; i++)
            {
                a = Mu_ef[i] * MuEf_Fix;
                f1 = rH / r[i] * (r[i] * r[i] - rB * rB) / (rH * rH - rB * rB);
                f2 = rB / r[i] * (rH * rH - r[i] * r[i]) / (rH * rH - rB * rB);
                c = (f1 * Tau_rz_H + f2 * Tau_rz_B) / a;
                d = Tau_rt_H * rH / r[i] / a;
                b = Math.Sqrt(c * c + d * d);
                Mu_eff[i] = Mu * Math.Exp((m - 1) * Math.Log(b)) / MuEf_Fix;
            }
        }
     
        public double
            D=0,L=0,H0=0,H=0,TETA=0,e=0,u=0,fi=0,Mu0=0,T0=0,Mu=0,b=0,m=0,n=0,T=0,c=0,Lm=0,p=0,uX=0,uZ=0,db=0,Q_M=0,W=0,AL_Z=0,
            AL_X=0,cs=0,sn=0,fd=0,fp=0,ff=0,tkz=0,tkx=0,z=0,pr=0,f1=0,f2=0,Ro=0,MASSA=0,T_Start=0,a_T=0,Lam_T=0,
            T_korp=0,T_screw=0,Al_korp=0,Al_screw=0,pd=0,dLm=0,st=0,dz=0,dn=0,Mu_e=0,I0=0,I1=0,I2=0,I3=0,
            I4=0,I5=0,fMin=0,fMax=0,ffMin=0,ffMax=0,Eta_Z=0,Eta_X=0,dp_dx=0,dp_dz=0,sign_dp=0,Del=0,
            MuEf_Fix=0,g_=0,a_=0,b_=0,b_a=0,Q=0,Q_B=0,Q_B_X=0,Q_ut=0,Mu_delta=0,Q_a=0,Mu_a=0,func=0,
            Qj_delta=0,Q_delta=0,Q_delta0=0,dQ_delta=0,tQ_delta=0,Volume=0,fi_=0,uz_=0,W_st=0,
            W_fin=0,S_st=0,S_Fin=0,SQ=0,rH=0,rB=0,Tau_rz_B=0,Tau_rz_H=0,
            Tau_rt_H=0,Tau_rt_B=0;

        public int
            i=0,j=0,k=0,s=0,DriverVar=0,ModeVar=0,ErrorNumber=0,nL=0,nLm=0,ic=0,nPrint=0,n_Eta=0,
            itera=0,nSect=0,nSectOld=0,test=0,i_Alfa=0,iS_korp=0;

        public double[]
            mf = new double[61],mb = new double[61],Y_0 = new double[61],
            Line_0 = new double[61],Eta = new double[61],Mu_ef = new double[61],
            ZM = new double[61],TM = new double[61],PM = new double[61],
            Gamma_X = new double[61],Gamma_Z = new double[61],v_X = new double[61],
            v_Z = new double[61],Mu_eff = new double[61], Gamma_XX = new double[61],
            Gamma_ZZ = new double[61],vX_N = new double[61],vZ_N = new double[61],
            M_TETA = new double[61],M_P = new double[61],M_Q = new double[61],
            M_T = new double[61],r = new double[61];

        public bool
            IterEnd=false,transit=false;
        public RESULT Res = new RESULT();

        public bool 
            Start_PL=false,pr_T=false;

        public double
            W0=0,X_PL=0,H_pr=0,F1dXdZ=0,F2dXdZ=0,v_SZ=0,dHdZ=0,G_k=0,Lb=0,C_S=0,dQ_rasp=0,dT=0,dX_PL=0,
            Q_memory=0,T_Min=0,T_Max=0,p_Min=0,p_Max=0,X_Min=0,X_Max=0,Q_b_=0;

        public double[]
            P_OTN = new double[61],T_OTN = new double[61],
            X_OTN = new double[61],LAM_GR = new double[61];
        public int 
            ii=0;

        public double
            DLINA=0,AL_W_k=0,Del_k=0,T_W_k=0,Q_W_k=0,TB_k=0,q_Wk=0,TB_s=0,q_Ws=0;
        public int 
            iCYL=0;

        public DOP_DATA DOP = new DOP_DATA();
        public Byte i_Menu,j_Menu,i_print;
        public bool VMenuResult=false,New=false;
        public string[] sZ = new string[9];
        public string[] sC = new string[9];

        public int iSect=0;
        public double[] Lambda = new double[41];
        public DATA_ DataRec = new DATA_();
        public SECT[] SCT = new SECT[29];
        public CYLINDER[] CYL = new CYLINDER[14];
        public S_CUT S_C = new S_CUT();
        public S_SM S_S = new S_SM(); 
        public S_BAR S_B = new S_BAR();
        
        public double[] 
            aSpline = new double[61],bSpline = new double[61],
            cSpline = new double[61],dSpline = new double[61];


        //Рузультаты
        public string table = "";

        public List<double[]> PM_LIST = new List<double[]>();
        public List<double[]> TM_LIST = new List<double[]>();
        public List<double[]> X_OTN_LIST = new List<double[]>();
        public List<List<double>> X_PL_LIST = new List<List<double>>();
        public List<double[]> ZM_LIST = new List<double[]>();

        public List<List<Vector2>> PZ = new List<List<Vector2>>();
        public List<List<Vector2>> TZ = new List<List<Vector2>>();
        public List<List<Vector2>> XZ = new List<List<Vector2>>();
        public List<List<List<double>>> ZXPT = new List<List<List<double>>>();
    }
}