using System;
using UnityEngine;

public static class CalenderControlActions
{
    /// <summary>
    /// Возвращает дельту профиля толщины материала под давлением контризгиба
    /// </summary>
    /// <param name="r"> Сила на валок в ньютонах </param>
    /// <param name="b"> Расстояние до второй опоры </param>
    /// <param name="a"> Расстояние до первой опоры </param>
    /// <param name="z"> Координата по ширине </param>
    /// <param name="w"> Длина основного валка </param>
    /// <param name="E"> Модуль упругости материала валка </param>
    /// <param name="Db"> Диаметр бочки валка </param>
    /// <param name="Dn"> Диаметр шейки валка </param>
    /// <param name="Dh"> Диаметр сквозного отверстия валка </param>
    public static double Curve(
        double r,
        double b,
        double a,
        double z,
        double w,
        double E,
        double Db,
        double Dn,
        double Dh)
    {
        double Ib = Math.PI / 4 * (Math.Pow(Db, 4) / 16 - Math.Pow(Dh, 4) / 16);
        double Iw = Math.PI / 4 * (Math.Pow(Dn, 4) / 16 - Math.Pow(Dh, 4) / 16);
        double part1 = r * b / (2 * E * Ib);
        double part2 = Math.Pow(z, 2) - w * z - a * (w + (Ib / Iw) * a);
        return part1 * part2 * 1000;
    }

    /// <summary>
    /// Возвращает дельту профиля толщины материала под воздействием перекрещивания
    /// </summary>
    /// <param name="x"> Расстояние между валками </param>
    /// <param name="f"> Ширина пленки </param>
    /// <param name="w"> Ширина валка </param>
    /// <param name="d"> Диаметр валка </param>
    /// <param name="z"> Координат по ширине </param>
    public static double Cross(
        double x,
        double f,
        double w,
        double d,
        double z)
    {
        double part1 = Math.Sqrt(Math.Pow(x * f / w, 2) + Math.Pow(d, 2)) - d;
        double part2 = Math.Pow(1 - (2 * z / f), 2);
        return part1 * part2 * 1000;
    }

    /// <summary>
    /// Возвращает дельту профиля толщины материала под воздействием бомбировки
    /// </summary>
    /// <param name="w"> Ширина валка </param>
    /// <param name="f"> Ширина пленки </param>
    /// <param name="h"> Высота выпуклости валка </param>
    /// <param name="z"> Координата по ширине профиля </param>
    public static double Camber(
        double w,
        double f,
        double h,
        double z)
    {
        double alpha = Math.PI / 180 * 20 + 7 / 18f * Math.PI * (w - f) / w;
        double beta = Math.PI / 180 * 160 - 7 / 18f * Math.PI * (w - f) / w;
        double part1 = h / (1 - Math.Sin(alpha));
        double part2 = Math.Sin(alpha + (beta - alpha) / w * (z + (w - f) / 2)) - Math.Sin(alpha);
        return part1 * part2;
    }
}