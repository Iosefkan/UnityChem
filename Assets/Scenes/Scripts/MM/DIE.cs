using System;
using Types;

namespace Program
{
    class DIE
    {
        public DIE(){}

        public void init(InitData initData)
        {
            init(initData.res, initData.fluxData, initData.S_K);
        }

        public void init(RESULT res, FluxData dat, S_KG s_kg)
        {
            table = "";

            Res = res;
            DAT = dat;
            S_K = s_kg;
            
            for (int i = 0; i < S_K.Num_S; ++i)
            {
                SEC[i] = S_K.S[i];
            }

            first = true;

            Mu0 = Res.Mu0;
            Bn = Res.b;
            m = Res.n;
            T0 = Res.T0;
            A = Res.a;
            LM = Res.Lam;

            kZ = S_K.Num_S;
            MZ[0] = 0;
            MR[0] = SEC[0].D_st / 2;
            for (int i = 1; i <= kZ; i++)
            {
                MZ[i] = MZ[i - 1] + SEC[i-1].L_sect;
                MR[i] = SEC[i-1].D_fin / 2;
                kZR[i-1] = SEC[i-1].n_cykle;
                TZ[i] = SEC[i-1].T_st;
            }
            for (int i = 0; i <= kZ; i++)
            {
                MZ[i] = MZ[i] / 1000;
                MR[i] = MR[i] / 1000;
            }

            first = true;
            second = true;

            for (int i = 1; i <= kZ; i++)
            {
                if (S_K.S[i-1].PRIZNAK == 2)
                {
                    if (first)
                    {
                        Num_sect = i;
                        first = false;
                    }
                    MRB[i] = S_K.S[i-1].D_B_fin / 2 / 1000;
                    TBZ[i] = S_K.S[i-1].T_B;
                }
            }
            MRB[Num_sect - 1] = S_K.S[Num_sect-1].D_B_st / 2 / 1000;
            TBZ[Num_sect - 1] = S_K.S[Num_sect-1].T_B;

            first = true;
            table += "Даные рассчитанные с модуля QPT:\n";

            for (int i = 0; i <= Res.n_Q; i++)
            {
                table += "i=" + i.ToString() + " Q=" + Res.MQ[i].ToString() + " см^3/сек; p=" + Res.MP[i].ToString() + " МПа; T=" + Res.MT[i].ToString() + " градусов.\n";
            }

            n_Omit = 1;
            jM = 0;
            for (int i = 1; i <= S_K.Num_S; i++)
            {
                jM = jM + S_K.S[i-1].n_cykle;
            }
            if (jM > 40)
            {
                n_Omit = (int)(jM / 40.0) + 1;
            }
            Final = false;

            for (i_Q = 0; i_Q <= Res.n_Q; i_Q++) // { Steps by Q }
            {
                first = true;
                Q = Res.MQ[i_Q] * 1E-06;
                TH = Res.MT[i_Q];
            Met4:
                if (first)
                {
                    stepR(0, 1, ref DAT.N, DAT.AB, DAT.AH, ref AL);
                    if (first)
                    {
                        stepR(0, 1, ref DAT.iR, DAT.kB, DAT.kH, ref R);
                    }
                    for (int i = 0; i <= DAT.N; i++)
                    {
                        T[i] = TH;
                        BP[i] = 0;
                        AL[i] = AL[i] / AL[DAT.N];
                    }
                    for (int i = 0; i <= DAT.iR; i++)
                    {
                        TR[i] = TH;
                        R[i] = R[i] / R[DAT.iR];
                    }
                    p = 0;
                    U = MR[0];
                    U = 4 * Q / Math.PI / (U * U * U); // Эффективная скорость сдвига
                    ROUND_();
                    if (!first)
                    {
                        goto Met4;
                    }
                }
                else
                {
                    if (second)
                    {
                        KT = 40;
                        SD[0] = 0;
                        SD[1] = 0.5;
                        dGamma = 0.5;
                        TAU[0] = 0;
                        for (int i = 2; i <= KT; i++)
                        {
                            dGamma = dGamma * 1.15;
                            SD[i] = SD[i - 1] + dGamma;
                        }
                        for (int i = 1; i <= KT; i++)
                        {
                            SD[i] = SD[i] / SD[KT] * 3000;
                            TAU[i] = Mu0 * Math.Exp(m * Math.Log(SD[i]));
                        }
                        TH = 0;
                        for (int i = 1; i <= DAT.N; i++)
                        {
                            TH = TH + (T[i - 1] + T[i]) / 2 * (AL[i] - AL[i - 1]);
                        }
                        for (int i = 0; i <= DAT.N; i++)
                        {
                            T[i] = TH;
                        }
                        for (int i = 0; i <= DAT.iR; i++)
                        {
                            TR[i] = TH;
                        }
                        second = false;
                    }
                    RING();
                    if (Final)
                    {
                        Q = Q_f * 1E-06;
                        PR = FixPr;
                        TH = spline(Q_f, 0, Res.n_Q, M_Q, Res.MT, ref PR);
                        table += "   i_Q=" + i_Q.ToString() + " Q=" + (Q * 1E+6).ToString() + "\n";
                    }
                }
                if (Final)
                {
                    break;
                }
            }

            for (int i = 0; i <= KT; i++)
            {
                TAU[i] = TAU[i] / 1000;
            }

            v_fin = 0;
            s_fin = Math.PI * (RR[DAT.iR] * RR[DAT.iR] - RR[0] * RR[0]);

            transit = true;
            d_f = 0.0001;
            Chorda(M_Q[0], (M_Q[Res.n_Q] - M_Q[0]), 0.001, ref Q_f, ref d_f, Dif);
            i = S_K.Num_S;
            s_fin = Math.PI * (MR[i] * MR[i] - MRB[i] * MRB[i]);
            v_fin = Q_f * 1E-6 / s_fin * 60;
            PR = FixPr;
            T_f = spline(Q_f, 0, Res.n_Q, M_Q, T_out, ref PR);
            PR = FixPr;
            p_f = spline(Q_f, 0, Res.n_Q, M_Q, Res.MP, ref PR);
            PR = FixPr;
            T_f_in = spline(Q_f, 0, Res.n_Q, M_Q, Res.MT, ref PR);

            RES_f.Q_fin = Q_f;
            RES_f.v_fin = v_fin;
            RES_f.T_mid = T_f;
            RES_f.R_inter = S_K.S[S_K.Num_S-1].D_B_fin / 2;
            RES_f.delta = S_K.S[S_K.Num_S-1].D_fin / 2 - S_K.S[S_K.Num_S-1].D_B_fin / 2;

            table += "\nРАССЧИТАННЫЕ ПАРАМЕТРЫ ЭКСТРУЗИИ:\n" +
                     "Объемный расход: " + Q_f + " см^3/сек\n" +
                     "Линейная скорость экструдирования профиля: v = " + v_fin + " м/мин\n" +
                     "Площадь сечения канала головки перед выходом профиля S = " + s_fin * 10000 + "см^2\n" +
                     "Температура смеси при выходе из головки: " + T_f + " градусов.\n" +
                     "Температура смеси перед головкой: " + T_f_in + " градусов.\n" +
                     "Удельное давление перед головкой: p = " + p_f + " МПа";
        }

        //{ Разбивка с шагом по r}
        void stepR(double rB, double rH, ref int N, double kB, double kH, ref double[] R)
        {
            int i, c;
            double d;
            d = N;
            d = d / 2;
            if (d - (N / 2.0) > 0.1)
                N = N - 1;
            c = N / 2;
            d = 1;
            R[0] = 0;
            for (i = 1; i <= c; i++)
            {
                R[i] = R[i - 1] + d;
                d = d * kB;
            }
            for (i = 0; i <= c; i++)
                R[i] = rB + R[i] / R[c] * (rH - rB) / 2.0;
            R[N] = 0;
            d = 1;
            for (i = N - 1; i >= c; i--)
            {
                R[i] = R[i + 1] + d;
                d = d * kH;
            }
            for (i = N; i >= c; i--)
                R[i] = rH - R[i] / R[c] * (rH - rB) / 2.0;
        }
            
        public void ROUND_()
        {
            int i, jj, j;
            
            k = 0;
            for (i = 0; i <= DAT.N; i++)
            {
                time[i] = 0;
            }
            k_omit = n_Omit - 1;
            for (jZ = 1; jZ <= kZ; jZ++)
            {
                if (S_K.S[jZ-1].PRIZNAK == 2)
                {
                    first = false;
                    return;
                }
                T[DAT.N] = TZ[jZ];
                TR[DAT.iR] = TZ[jZ];
                j = kZR[jZ-1];
                z = MZ[jZ - 1];
                dz = (MZ[jZ] - z) / j;
                RH = MR[jZ - 1];
                DRH = (MR[jZ] - RH) / j;
                TETA = Math.Abs(Math.Atan(DRH / dz));
                SN = Math.Sin(TETA);
                CS = Math.Cos(TETA);
                for (jj = 0; jj <= j; jj++)
                {
                    if (TETA > 0.05)
                    {
                        RC2 = RH / SN;
                    }
                    CC = Math.PI * RH * RH * RH / Q;
                    TAUH = F1();
                    for (i = 0; i <= DAT.N; i++)
                    {
                        R2[i] = curve(AL[i], 0, DAT.iR, ALR, R, ref PR) * RH;
                        G2[i] = Math.Abs(TAUH * R2[i] / RH);
                        if (Math.Abs(G2[i]) > 1E-06)
                        {
                            G2[i] = G2[i] * Math.Exp((Math.Log(G2[i] / Mu0) + Bn * (T[i] - T0)) / m) * R2[i] / RH;
                        }
                        if (TETA > 0.05)
                        {
                            TETA2[i] = Math.Atan(R2[i] / (RC2 * CS));
                        }
                        if (jj > 0)
                        {
                            if (i > 0)
                            {
                                if (TETA > 0.05)
                                {
                                    Fi2 = (TETA2[i] + TETA1[i]) / 2.0;
                                    Fi1 = (TETA2[i - 1] + TETA1[i - 1]) / 2.0;
                                    DTETA = Fi2 - Fi1;
                                    DX[i] = (RC2 + RC1) / 2.0 * DTETA;
                                    DB[i] = 2 * Math.PI / 3.0 * (RC2 * RC2 * RC2 - RC1 * RC1 * RC1) * (Math.Cos(Fi1) - Math.Cos(Fi2));
                                    DB[i] = Math.Abs(DB[i]) / ((AL[i] - AL[i - 1]) * Q);
                                }
                                else
                                {
                                    DX[i] = (R2[i] - R2[i - 1] + R1[i] - R1[i - 1]) / 2.0;
                                    DB[i] = Math.PI * ((R2[i] - R2[i - 1]) * (R2[i] + R2[i - 1]) + (R1[i] - R1[i - 1]) * (R1[i] + R1[i - 1])) / 2.0 * dz;
                                    DB[i] = DB[i] / ((AL[i] - AL[i - 1]) * Q);
                                }
                                G[i] = (G2[i] + G2[i - 1] + G1[i] + G1[i - 1]) / 4.0;
                            }
                        }
                    }
                    for (i = 0; i <= DAT.N; i++)
                    {
                        R1[i] = R2[i];
                        G1[i] = G2[i];
                        TETA1[i] = TETA2[i];
                        if (jj > 0 && i > 0)
                        {
                            TNu = (T[i] + T[i - 1]) / 2.0;
                        }
                    }
                    RC1 = RC2;
                    DP2 = TAUH / RH;
                    if (jj > 0)
                    {
                        TRTV(2, 1, DAT.N, ref T, DX, G, DB, 0, 0, 0, 0, TMP, TPL, SIGMA);
                        PR = FixPr;
                        for (i = 0; i <= DAT.iR; i++)
                        {
                            TR[i] = spline(R[i] * RH, 0, DAT.N, R2, T, ref PR);
                        }
                        p = p + (DP1 + DP2) * dz;
                    }
                    if (jj < j)
                    {
                        z = z + dz;
                        RH = RH + DRH;
                        DP1 = DP2;
                        k_omit = k_omit + 1;
                        if (k_omit == n_Omit)
                        {
                            k_omit = 0;
                        }
                    }
                }
                for (i = 0; i <= DAT.N; i++)
                {
                    rOtn[i] = R2[i] / RH;
                }
            }
            
            if (i_Q == 0)
            {
                table += "        i       Q       p        T      T_out\n";
                table += "        -   см^3/сек   МПа       гр.      гр.\n";
            }
            T_out[i_Q] = 0;
            for (i = 1; i <= DAT.N; i++)
            {
                T_out[i_Q] = T_out[i_Q] + (T[i - 1] + T[i]) / 2 * (AL[i] - AL[i - 1]);
            }
            M_Q[i_Q] = Q * 1E+6;
            M_P[i_Q] = p * 1E-6;
            M_T[i_Q] = Res.MT[i_Q];
            table += $"     {i_Q}   {M_Q[i_Q]}    {M_P[i_Q]}   {M_T[i_Q]}   {T_out[i_Q]}";
            
            return;
        }

        double F1()
        {
            int i;
            double F, A1=0, A2, g1=0, g2, D;
            
            VZ[DAT.iR] = 0;
            ALR[DAT.iR] = 0;
            for (i = DAT.iR; i >= 0; i--)
            {
                if (R[i] == 0)
                    g2 = 0;
                else
                    g2 = Math.Exp((Math.Log(R[i]) + Bn * (TR[i] - T0)) / m);
                A2 = R[i] * R[i] * g2;
                if (i == DAT.iR)
                    goto m1;
                D = R[i + 1] - R[i];
                VZ[i] = VZ[i + 1] + (g1 + g2) / 2.0 * D * RH;
                ALR[i] = ALR[i + 1] + (A1 + A2) / 2.0 * D;
                m1:
                g1 = g2;
                A1 = A2;
            }
            F = CC * ALR[0];
            for (i = 0; i <= DAT.iR; i++)
            {
                ALR[i] = ALR[i] / F;
                VZ[i] = VZ[i] / F;
                ALR[i] = CC * (ALR[i] - R[i] * R[i] * VZ[i] / RH);
            }
            for (i = DAT.iR; i >= 0; i--)
                ALR[i] = 1 - ALR[i] / ALR[0];
            return Mu0 * Math.Exp(-m * Math.Log(F));
        }

        public double curve(double x, int iH, int iK, double[] mx, double[] my, ref double Pr)
        {
            int i=0, j;
            double r1=0, r2, x1, x2, y2;
            for (j = iH + 1; j < iK; j++)
            {
                r2 = x - mx[j];
                if (j > iH + 1 && Math.Abs(r2) > Math.Abs(r1))
                {
                    j = iK - 1;
                }
                else
                {
                    i = j;
                    r1 = r2;
                }
            }
            r1 = x - mx[i];
            x1 = mx[i - 1] - mx[i];
            x2 = mx[i + 1] - mx[i];
            y2 = my[i + 1] - my[i];
            r2 = (y2 / x2 - (my[i - 1] - my[i]) / x1) / (x2 - x1);
            Pr = y2 / x2 + r2 * (2 * r1 - x2);
            return r1 * (y2 / x2 + r2 * (r1 - x2)) + my[i];
        }

        void TRTV(int G0, int GN, int N, ref double[] T, double[] DY, double[] Q, double[] DTAU, 
                    double T0, double TN, double AL0, double ALN, Func<double, int, double> A, 
                    Func<double, int, double> L, Func<double, double> SIGMA)
        {
            int i, j, c;
            double r1, r2=0, r3, r4, r5, r6, r7, r8, d;
            double[] B = new double[61];
            double[] M = new double[61];
            double[] DT = new double[61];

            if (G0 == 3)
            {
                r1 = -AL0;
                r2 = AL0 * (T[0] - T0);
            }
            else
            {
                r1 = 0;
            }
            if (G0 == 2)
            {
                r2 = -AL0;
            }
            r7 = 0;
            for (i = 0; i <= N - 1; i++)
            {
                r3 = (T[i] + T[i + 1]) / 2.0;
                r4 = A(r3, i) / (DY[i + 1] * DY[i + 1]) * DTAU[i + 1];
                r5 = L(r3, i) / DY[i + 1];
                r8 = Q[i + 1] / 2.0 * DY[i + 1];
                d = SIGMA(r4);
                r6 = -r5 * (0.5 / r4 + d);
                r4 = r5 * (T[i + 1] - T[i]);
                M[i] = r5 * d;
                if (i == 0 && G0 == 1)
                {
                    goto m1;
                }
                B[i] = r1 + r6;
                DT[i] = r2 - r4 - r7 - r8;
                if (i == 1 && G0 == 1)
                {
                    DT[1] = DT[1] - M[0] * T0;
                }
                if (i == N - 1 && GN == 1)
                {
                    DT[i] = DT[i] - M[i] * TN;
                }
            m1:
                r2 = r4;
                r1 = r6;
                r7 = r8;
            }
            if (GN > 1)
            {
                c = N;
                if (GN == 2)
                {
                    B[N] = r1;
                    DT[N] = r2 - ALN - r7;
                }
                else
                {
                    B[N] = r1 - ALN;
                    DT[N] = r2 + ALN * (T[N] - TN) - r7;
                }
            }
            else
            {
                c = N - 1;
            }
            if (G0 == 1)
            {
                j = 1;
            }
            else
            {
                j = 0;
            }
            tridag(j, c, B, M, ref DT);
            for (i = j; i <= c; i++)
            {
                T[i] = T[i] + DT[i];
            }
            if (G0 == 1)
            {
                T[0] = T[0] + T0;
            }
            if (GN == 1)
            {
                T[N] = T[N] + TN;
            }
        }

        void tridag(int H, int K, double[] B, double[] M, ref double[] C)
        {
            double r;
            int i;
            r = B[H];
            C[H] = C[H] / r;
            for (i = H; i < K; i++)
            {
                B[i] = M[i] / r;
                r = B[i + 1] - M[i] * B[i];
                C[i + 1] = (C[i + 1] - M[i] * C[i]) / r;
            }
            for (i = K - 1; i >= H; i--)
            {
                C[i] = C[i] - B[i] * C[i + 1];
            }
        }

        double TMP(double T, int i)
        {
            return A;
        }

        double TPL(double T, int i)
        {
            return LM*(R1[i]+R1[i+1]+R2[i]+R2[i+1])/2.0/(R1[DAT.N]+R2[DAT.N]);
        }

        double SIGMA(double FO)
        {
            return 0.5;
        }

            double[] aSpline=new double[61],bSpline=new double[61],
                     cSpline=new double[61],dSpline=new double[61];
                     
        public double spline(double x, int iH, int iK, double[] xM, double[] yM, ref double Pr)
        {
            bool a, b, z;
            int i=0, j;
            double r=0, d;


            a = x > xM[iH];
            b = x < xM[iK];
            z = xM[iH] < xM[iK];
            if (Pr == FixPr)
                kSpline(iH, iK, xM, yM, ref aSpline, ref bSpline, ref cSpline, ref dSpline);
            if ((a && b) || (!a && !b))
            {
                for (j = iH; j <= iK; j++)
                {
                    d = x - xM[j];
                    if (j > iH && Math.Abs(d) > Math.Abs(r))
                    {
                        j = iK;
                    }
                    else
                    {
                        i = j;
                        r = d;
                    }
                }
                if ((z && r < 0) || (!z && r >= 0 && i > iH))
                {
                    i = i - 1;
                }
                d = x - xM[i];
                Pr = bSpline[i] + d * (2 * cSpline[i] + d * 3 * dSpline[i]);
                return aSpline[i] + d * (bSpline[i] + d * (cSpline[i] + d * dSpline[i]));
            }
            else if ((z && !a) || (!z && a))
            {
                Pr = bSpline[iH];
                return aSpline[iH] + Pr * (x - xM[iH]);
            }
            else
            {
                d = xM[iK] - xM[iK - 1];
                Pr = bSpline[iK - 1] + d * (2 * cSpline[iK - 1] + d * 3 * dSpline[iK - 1]);
                return yM[iK] + Pr * (x - xM[iK]);
            }
        }

        public void kSpline(int iH, int iK, double[] xM, double[] yM, ref double[] a, ref double[] b, ref double[] c, ref double[] d)
        {
            int i;
            double dx1=0, dx2, dy1=0, dy2;
            double[] MD = new double[60];
            double[] BD = new double[60];

            for (i = iH; i < iK; i++)
            {
                a[i] = yM[i];
                dx2 = xM[i + 1] - xM[i];
                dy2 = yM[i + 1] - yM[i];
                if (i > iH)
                {
                    MD[i] = dx2;
                    BD[i] = 2 * (dx1 + dx2);
                    c[i] = 3 * (dy2 / dx2 - dy1 / dx1);
                }
                dx1 = dx2;
                dy1 = dy2;
            }

            Tridg(iH + 1, iK - 1, BD, MD, ref c);

            c[iH] = 0;
            c[iK] = 0;

            for (i = iH; i < iK; i++)
            {
                dx2 = xM[i + 1] - xM[i];
                b[i] = (yM[i + 1] - yM[i]) / dx2 - dx2 * (c[i + 1] + 2 * c[i]) / 3.0;
                d[i] = (c[i + 1] - c[i]) / 3.0 / dx2;
            }
        }

        void Tridg(int H, int K, double[] B, double[] M, ref double[] C)
        {
            double r;
            r = B[H];
            C[H] = C[H] / r;
            for (int i = H; i < K; i++)
            {
                B[i] = M[i] / r;
                r = B[i + 1] - M[i] * B[i];
                C[i + 1] = (C[i + 1] - M[i] * C[i]) / r;
            }
            for (int i = K - 1; i >= H; i--)
            {
                C[i] = C[i] - B[i] * C[i + 1];
            }
        }

        void RING()
        {
            int jj, k, j, i;
            k = Num_sect;
            for (jZ = Num_sect; jZ <= kZ; jZ++) //{Начало циклов по номерам секций канала}
            {
                T[DAT.N] = TZ[jZ];
                TR[DAT.iR] = TZ[jZ];
                T[0] = Res.MT[i_Q];
                TR[0] = T[0];
                j = S_K.S[jZ-1].n_cykle;
                z = MZ[jZ - 1];
                dz = (MZ[jZ] - z) / j;
                RH = MR[jZ - 1];
                DRH = (MR[jZ] - RH) / j;
                for (jj = 0; jj <= j; jj++) //{ Начало циклов вдоль секции }
                {
                    rB = LineInterpol(z, 0, kZ, MZ, MRB, ref PR);
                    RH = LineInterpol(z, 0, kZ, MZ, MR, ref PR);
                    stepR(rB, RH, ref DAT.iR, DAT.kB, DAT.kH, ref RR); //{ Разбивка с шагом по r}
                    if (jZ == 3 && jj == 0) // { Первичная разбивка по r }
                    {
                        for (i = 0; i <= DAT.N; i++)
                        {
                            R2[i] = rB + i * (RH - rB) / DAT.N;
                        }
                    }
                    
                    for (i = 0; i <= DAT.iR; i++)
                    {
                        TR[i] = LineInterpol(RR[i], 0, DAT.N, R2, T, ref PR);
                        TR[i] = Math.Exp(Bn * (TR[i] - T0));
                    }
                    if (jZ == 3 && jj == 0)
                    {
                        for (i = 1; i <= 2; i++)
                        {
                            X[i-1] = TAU[KT];
                            D_X[i-1] = 0.2 * X[i-1];
                            TX[i-1] = 0.001 * X[i-1];
                        }
                    }
                    else
                    {
                        for (i = 1; i <= 2; i++)
                        {
                            D_X[i-1] = 0.1 * X[i-1];
                            TX[i-1] = 0.0005 * X[i-1];
                        }
                    }
                    dX_ = D_X[0];
                    tx_ = TX[0];

                    MinX1X2S(dX_, tx_, F, ref X, ref Y_, 2);

                    for (i = 0; i <= DAT.iR; i++)
                    {
                        ALR[i] = ALR[i] / ALR[DAT.iR];
                    }
                    dp = 2 * (X[1] * RH + X[0] * rB) / (RH * RH - rB * rB);
                    for (i = 0; i <= DAT.N; i++)
                    {
                        PR = FixPr;
                        R2[i] = spline(AL[i], 0, DAT.iR, ALR, RR, ref PR);
                        PR = (X[2-1] * RH - dp / 2 * (RH * RH - R2[i] * R2[i])) / R2[i];
                        pr1 = FixPr;
                        Gamma[i] = LineInterpol(Math.Abs(PR) * Math.Exp(Bn * (T[i] - T0)), 0, KT, TAU, SD, ref pr1) * Sign(PR);
                        G2[i] = Math.Abs(PR * Gamma[i]);
                    }
                    if (jj > 0)
                    {
                        p = p + (dp + DP1) / 2 * dz;
                        for (i = 1; i <= DAT.N; i++)
                        {
                            DDX[i] = (R1[i] - R1[i - 1] + R2[i] - R2[i - 1]) / 2;
                            G[i] = (G1[i] + G1[i - 1] + G2[i] + G2[i - 1]) / 4;
                            G[i] = G[i] * (R1[i] + R1[i - 1] + R2[i] + R2[i - 1]) / 2 / (R1[DAT.N] + R2[DAT.N]);
                            dTAU[i] = Math.PI * dz / Q / (1 / (R1[i] * R1[i] - R1[i - 1] * R1[i - 1]) + 1 / 
                                                (R2[i] * R2[i] - R2[i - 1] * R2[i - 1])) * 2 / (AL[i] - AL[i - 1]);
                        }
                        TRTV(1, 1, DAT.N, ref T, DDX, G, dTAU, 0, 0, 0, 0, TMP, TPL, SIGMA);
                        pr1 = FixPr;
                        for (i = 0; i <= DAT.iR; i++)
                        {
                            TR[i] = spline(RR[i], 0, DAT.N, R2, T, ref pr1);
                        }
                        for (i = 1; i <= DAT.N; i++)
                        {
                            time[i] = time[i] + dTAU[i]; //‚Ћ‡ЊЋ†ЌЋ ‡„…‘њ Ћ€ЃЉЂ !!! 
                        }
                    }
                    if (jj < j)
                    {
                        z = z + dz;
                        RH = RH + DRH;
                        DP1 = dp;
                        for (i = 0; i <= DAT.N; i++)
                        {
                            R1[i] = R2[i];
                        }
                        G1[i] = G2[i];
                    }
                    X1[1] = X1[2];
                    X1[2] = X[1-1];
                    X2[1] = X2[2];
                    X2[2] = X[2-1];
                    ZZ[1] = ZZ[2];
                    ZZ[2] = z - dz;
                    if (jj > 0 && jj < j)
                    {
                        X[2-1] = LineInterpol(z, 1, 2, ZZ, X2, ref pr1);
                        X[1-1] = LineInterpol(z, 1, 2, ZZ, X1, ref pr1);
                    }
                }
            }
            T_out[i_Q] = 0;
            for (i = 1; i <= DAT.N; i++)
            {
                T_out[i_Q] = T_out[i_Q] + (T[i - 1] + T[i]) / 2 * (AL[i] - AL[i - 1]);
            }
            M_Q[i_Q] = Q * 1E+6;
            M_P[i_Q] = p * 1E-6;
            M_T[i_Q] = Res.MT[i_Q];
            table += "i=" + i_Q.ToString() + " Q=" + M_Q[i_Q].ToString() + " см^3/сек; p=" + M_P[i_Q].ToString() + " МПа; T=" + M_T[i_Q].ToString() + "; T_out=" + T_out[i_Q].ToString() + " градусов.\n";
        }

        public static double LineInterpol(double x, int iH, int iK, double[] mx, double[] my, ref double Pr)
        {
            int i=0, j;
            double a=0, b;
            
            for (j = iH; j < iK; j++)
            {
                b = x - mx[j];
                if (j > iH && Math.Abs(b) > Math.Abs(a))
                {
                    j = iK - 1;
                }
                else
                {
                    i = j;
                    a = b;
                }
            }
            
            b = x - mx[i];
            if (b < 0 && i > iH)
            {
                i = i - 1;
                b = x - mx[i];
            }
            
            Pr = (my[i + 1] - my[i]) / (mx[i + 1] - mx[i]);
            return my[i] + Pr * b;
        }

        public void MinX1X2S(double dx, double tx, Func<double> f, ref double[] x, ref double y, int k)
        {
            int i, j, iMin=0, jMin=0, iq, jq, m=0, n=0, q;
            double x1, x2, yMin=0, d;
            double[,] y1 = new double[7, 7];
            double[,] y2 = new double[7, 7];
            
            d = dx;
            q = 0;
            
        m1:
            if (q == 0)
                yMin = 1E+30;
            
            x1 = x[0];
            x2 = x[1];
            iq = 0;
            jq = 0;
            
            for (j = -k; j <= k; j++)
            {
                if (q == 1)
                    n = j + jMin;
                if (q == 2)
                    n = j / 2 + jMin;
                
                x[1] = x2 + d * j;
                
                for (i = -k; i <= k; i++)
                {
                    if (q == 1)
                        m = i + iMin;
                    if (q == 2)
                        m = i / 2 + iMin;
                    
                    x[0] = x1 + d * i;
                    
                    if (q == 0 || (q == 1 && (Math.Abs(n) > k || Math.Abs(m) > k)) ||
                            (q == 2 && (i - (i / 2) * 2 != 0 || j - (j / 2) * 2 != 0)))
                    {
                        y2[i+3, j+3] = f();
                        y = y2[i+3, j+3];
                        
                        if (y < 5E-05)
                            goto m2;
                        
                        if (y < yMin)
                        {
                            iq = i;
                            jq = j;
                            yMin = y;
                        }
                    }
                    else
                    {
                        y2[i+3, j+3] = y1[m+3, n+3];
                    }
                }
            }
            
            q = 1;
            
            for (i = -k; i <= k; i++)
            {
                for (j = -k; j <= k; j++)
                {
                    y1[i+3, j+3] = y2[i+3, j+3];
                }
            }
            
            iMin = iq;
            jMin = jq;
            x[0] = iMin * d + x1;
            x[1] = jMin * d + x2;
            y = yMin;
            
            if (d < tx)
                goto m2;
            
            if (iMin == -k || iMin == k || jMin == -k || jMin == k)
                goto m1;
            
            q = 2;
            d = d / 2;
            goto m1;
            
        m2:
            return;
        }

        double F()
        {
            double DR, Int, A, G1=0, G2, F1=0, F2, S, D;
            int i;
            D = RH * RH;
            S = D - rB * rB;
            ALR[0] = 0;
            VZ[0] = 0;
            Int = 0;
            for (i = 0; i <= DAT.iR; i++)
            {
                A = -((X[2-1] * RH + X[1-1] * rB) * (RR[i] * RR[i] - rB * rB) / S - X[1-1] * rB) / RR[i];
                G2 = LineInterpol(Math.Abs(A) * TR[i], 0, KT, TAU, SD, ref PR) * Math.Sign(A);
                F2 = G2 * RR[i] * RR[i];
                if (i > 0)
                {
                    DR = RR[i] - RR[i - 1];
                    VZ[i] = VZ[i - 1] + (G1 + G2) / 2 * DR;
                    Int = Int + (F1 + F2) / 2 * DR;
                    ALR[i] = (VZ[i] * RR[i] * RR[i] - Int) * Math.PI / Q;
                }
                G1 = G2;
                F1 = F2;
            }
            
            return Math.Pow(VZ[DAT.iR] * Math.PI * S / Q, 2) + Math.Pow(ALR[DAT.iR] - 1, 2);
        }

        public int Sign(double x)
        {
            if (Math.Abs(x) < 1E-10)
                return 0;
            else if (x < 0)
                return -1;
            else
                return 1;
        }

        public double Dif()
        {
            double pr, a, b;
            pr = FixPr;
            a = spline(Q_f, 0, Res.n_Q, M_Q, Res.MP, ref pr);
            pr = FixPr;
            b = spline(Q_f, 0, Res.n_Q, M_Q, M_P, ref pr);
            return a - b;
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
            } while (!((Math.Abs(x2 - x1) < tx && transit) || (Math.Abs(y) <= ty) || v));
            if (v) transit = true;
        }

        public const int FixPr=4738;

        public int
            kZ,i,j,jZ,jj,i_Q,k,k_min,k_max,ModeVar,DriverVar,ErrorNumber,
            n_Omit,jM,k_omit,KT,kk,ii,Num_sect,jMax;

        public double
            Mu0,Bn,m,TH,Q,T0,A,LM,z,dz,p,U,DTETA,Fi1,Fi2,PR,RH,DRH,TAUH,TAUW,
            RC1,RC2,TETA,SN,CS,DP1,DP2,CC,TNu,Vzap,fMin,fMax,sMin,sMax,dMin,
            dMax,xMin,xMax,E_R,T_eq,t_ind_eq,d_f,Q_f,p_f,T_f,Vul_f,Vul_f_S,
            T_f_in,s_fin,v_fin,dGamma,rB,dX_,tx_,Y_,dp,pr1;

        public double[]
            VZ = new double[61],G1 = new double[61],G2 = new double[61],TETA1 = new double[61],
            TETA2 = new double[61],BP = new double[61],T = new double[61],DX = new double[61],
            DB = new double[61],G = new double[61],AL = new double[61],ALR = new double[61],
            R = new double[61],TR = new double[61],R1 = new double[61],R2 = new double[61],
            rOtn = new double[61],MZ = new double[61],MR = new double[61],TZ = new double[61],
            ZM = new double[61],PM = new double[61],TAUM = new double[61],M_Q = new double[61],
            M_P = new double[61],M_T = new double[61],T_out = new double[61],time = new double[61],
            SD = new double[61],TAU = new double[61],MRB = new double[61],TBZ = new double[61],
            RR = new double[61],Gamma = new double[61],DDX = new double[61], dTAU = new double[61],
            ZZ = new double[61],X1 = new double[61],X2 = new double[61];

        public int[]
            kZR = new int[60];

        public Byte
            iMenu,i_Menu,j_Menu;

        public FluxData
            DAT = new FluxData();

        public SECTIONS[]
            SEC = new SECTIONS[29];
        public RESULT
            Res = new RESULT();

        public S_KG
            S_K = new S_KG();
            
        public  RESULT_D
            RES_D = new RESULT_D();

        public Res_fin
            RES_f = new Res_fin();
            
        public bool
            VMenuResult,New,transit,Final,
            first,second;

        public double[]
            X = new double[5],D_X = new double[5],TX = new double[5];

        public string table = "";
    }
}