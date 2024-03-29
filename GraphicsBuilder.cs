using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using ScottPlot;
using static Org.BouncyCastle.Math.EC.ECCurve;

static class GraphicsBuilder
{
    public static string BuildTeacherSubjectDistribution(Dictionary<string, long> res)
    {
        var plt = new ScottPlot.Plot(700, 600);

        double[] values = new double[res.Count];
        string[] subjects = new string[res.Count];
        res.Keys.CopyTo(subjects, 0);

        for (int i = 0; i < res.Count; i++)
        {
            values[i] = res[subjects[i]];
        }

        var pie = plt.AddPie(values);
        pie.SliceLabels = subjects;
        pie.ShowValues = true;
        plt.Legend();
        plt.Title($"Teachers-subject distribution"); 

        string filePath = "Teachers-subject distribution.png";

        plt.SaveFig(filePath);
        return filePath;
    }

    public static string BuildSpeciltyGeneralPerformance(Dictionary<string, decimal> res)
    {
        var plt = new ScottPlot.Plot(700, 600);

        string[] labels = new string[7]; 
        double[] ys = new double[7];
        res.Keys.CopyTo(labels, 0);

        int barCount = labels.Length;
        Random rand = new Random(0);
        double[] xs = DataGen.Consecutive(barCount);


        for (int i = 0; i < labels.Length; i++)
        {
            ys[i] = Math.Round((double)res[labels[i]], 1);
        }
        var bar = plt.AddBar(ys, xs);
         
        bar.ShowValuesAboveBars = true;
        plt.YLabel("Average mark");
        plt.XLabel("Specialty");

        plt.Grid(enable: false, lineStyle: LineStyle.Dot);

        plt.XTicks(xs, labels);
        string graphicPath = "Average Specialty Mark.png";

        plt.SaveFig(graphicPath);
        return graphicPath;
    }

    public static string BuildStudentPerformance(Dictionary<string, decimal> res, string fullname)
    {
        var plt = new ScottPlot.Plot(75 * res.Count, 400);

        string[] labels = new string[res.Count]; 
        double[] ys = new double[res.Count];
        res.Keys.CopyTo(labels, 0);

        int barCount = labels.Length;
        Random rand = new Random(0);
        double[] xs = DataGen.Consecutive(barCount);


        for (int i = 0; i < labels.Length; i++)
        {
            ys[i] = Math.Round((double)res[labels[i]], 1);
        }

        var bar = plt.AddBar(ys, xs);
         
        bar.ShowValuesAboveBars = true;
        plt.YLabel($"{fullname} performance");

        plt.Grid(enable: false, lineStyle: LineStyle.Dot);

        plt.XTicks(xs, labels);
        plt.XAxis.TickLabelStyle(rotation: 45);
        string graphicPath = $"{fullname} Performance.png";

        plt.SaveFig(graphicPath);
        return graphicPath;
    }

    public static string BuildSpecialtyPerformance(Dictionary<string, decimal> res, string specialty)
    {
        var plt = new ScottPlot.Plot(75 * res.Count, 400);

        string[] labels = new string[res.Count]; 
        double[] ys = new double[res.Count];
        res.Keys.CopyTo(labels, 0);

        int barCount = labels.Length;
        Random rand = new Random(0);
        double[] xs = DataGen.Consecutive(barCount);


        for (int i = 0; i < labels.Length; i++)
        {
            ys[i] = Math.Round((double)res[labels[i]], 1);
        }

        var bar = plt.AddBar(ys, xs);
         
        bar.ShowValuesAboveBars = true;
        plt.YLabel($"\"{specialty}\" performance");

        plt.Grid(enable: false, lineStyle: LineStyle.Dot);

        plt.XTicks(xs, labels);
        plt.XAxis.TickLabelStyle(rotation: 45);
        string graphicPath = $"{specialty} Performance.png";

        plt.SaveFig(graphicPath);
        return graphicPath;
    }

    public static string BuildStudentProgress(Dictionary<DateTime, decimal> res, string fullname)
    {
        var plt = new ScottPlot.Plot(1200, 600);

        Random rand = new Random(0);
        double[] ys = new double[res.Count];
        DateTime[] dates = new DateTime[res.Count];
        res.Keys.CopyTo(dates, 0);
        double[] xs = new double[ys.Length];
        string[] strDates = new string[res.Count];

        for (int i = 0; i < ys.Length; i++)
        {
            xs[i] = dates[i].ToOADate();
            ys[i] = Math.Round((double)res[dates[i]], 1);
            strDates[i] = dates[i].ToShortDateString();
        }

        plt.AddScatter(xs, ys);
        plt.XAxis.DateTimeFormat(true);
        plt.XTicks(xs, strDates);
        plt.XAxis.TickLabelStyle(rotation: 90);

        plt.Title($"{fullname} Progress");
        string filePath = $"{fullname} Progress.png";

        plt.SaveFig(filePath);

        return filePath;
    }
}