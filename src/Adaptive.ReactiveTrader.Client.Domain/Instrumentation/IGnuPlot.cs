using System;
using System.Collections.Generic;
using System.IO;

namespace Adaptive.ReactiveTrader.Client.Domain.Instrumentation
{
    public interface IGnuPlot : IDisposable
    {
        bool Hold { get; }
        void WriteLine(string gnuplotcommands);
        void Write(string gnuplotcommands);
        void Set(params string[] options);
        void Unset(params string[] options);
        bool SaveData(double[] Y, string filename);
        bool SaveData(double[] X, double[] Y, string filename);
        bool SaveData(double[] X, double[] Y, double[] Z, string filename);
        bool SaveData(int sizeY, double[] Z, string filename);
        bool SaveData(double[,] Z, string filename);
        void Replot();
        void Plot(string filenameOrFunction, string options = "");
        void Plot(double[] y, string options = "");
        void Plot(double[] x, double[] y, string options = "");
        void Contour(string filenameOrFunction, string options = "", bool labelContours = true);
        void Contour(int sizeY, double[] z, string options = "", bool labelContours = true);
        void Contour(double[] x, double[] y, double[] z, string options = "", bool labelContours = true);
        void Contour(double[,] zz, string options = "", bool labelContours = true);
        void HeatMap(string filenameOrFunction, string options = "");
        void HeatMap(int sizeY, double[] intensity, string options = "");
        void HeatMap(double[] x, double[] y, double[] intensity, string options = "");
        void HeatMap(double[,] intensityGrid, string options = "");
        void SPlot(string filenameOrFunction, string options = "");
        void SPlot(int sizeY, double[] z, string options = "");
        void SPlot(double[] x, double[] y, double[] z, string options = "");
        void SPlot(double[,] zz, string options = "");
        void Plot(List<StoredPlot> storedPlots);
        void SPlot(List<StoredPlot> storedPlots);
        void WriteData(double[] y, StreamWriter stream, bool flush = true);
        void WriteData(double[] x, double[] y, StreamWriter stream, bool flush = true);
        void WriteData(int ySize, double[] z, StreamWriter stream, bool flush = true);
        void WriteData(double[,] zz, StreamWriter stream, bool flush = true);
        void WriteData(double[] x, double[] y, double[] z, StreamWriter stream, bool flush = true);
        void SaveSetState(string filename = null);
        void LoadSetState(string filename = null);
        void HoldOn();
        void HoldOff();
        void Close();
        void WriteFile(string filename, string content);
        void DeleteFile(string filename);
        string GetCurrentDirectory();
    }

        enum PointStyles
    {
        Dot = 0,
        Plus = 1,
        X = 2,
        Star = 3,
        DotSquare = 4,
        SolidSquare = 5,
        DotCircle = 6,
        SolidCircle = 7,
        DotTriangleUp = 8,
        SolidTriangleUp = 9,
        DotTriangleDown = 10,
        SolidTriangleDown = 11,
        DotDiamond = 12,
        SolidDiamond = 13
    }
        public enum PlotTypes
    {
        PlotFileOrFunction,
        PlotY,
        PlotXY,
        ContourFileOrFunction,
        ContourXYZ,
        ContourZZ,
        ContourZ,
        ColorMapFileOrFunction,
        ColorMapXYZ,
        ColorMapZZ,
        ColorMapZ,
        SplotFileOrFunction,
        SplotXYZ,
        SplotZZ,
        SplotZ
    }

    public class StoredPlot
    {
        public string File = null;
        public string Function = null;
        public double[] X;
        public double[] Y;
        public double[] Z;
        public double[,] ZZ;
        public int YSize;
        public string Options;
        public PlotTypes PlotType;
        public bool LabelContours;

        public StoredPlot()
        {
        }

        public StoredPlot(string functionOrfilename, string options = "",
            PlotTypes plotType = PlotTypes.PlotFileOrFunction)
        {
            if (IsFile(functionOrfilename))
                File = functionOrfilename;
            else
                Function = functionOrfilename;
            Options = options;
            PlotType = plotType;
        }

        public StoredPlot(double[] y, string options = "")
        {
            Y = y;
            Options = options;
            PlotType = PlotTypes.PlotY;
        }

        public StoredPlot(double[] x, double[] y, string options = "")
        {
            X = x;
            Y = y;
            Options = options;
            PlotType = PlotTypes.PlotXY;
        }

        //3D data
        public StoredPlot(int sizeY, double[] z, string options = "", PlotTypes plotType = PlotTypes.SplotZ)
        {
            YSize = sizeY;
            Z = z;
            Options = options;
            PlotType = plotType;
        }

        public StoredPlot(double[] x, double[] y, double[] z, string options = "",
            PlotTypes plotType = PlotTypes.SplotXYZ)
        {
            if (x.Length < 2)
                YSize = 1;
            else
                for (YSize = 1; YSize < x.Length; YSize++)
                    if (x[YSize] != x[YSize - 1])
                        break;
            Z = z;
            Y = y;
            X = x;
            Options = options;
            PlotType = plotType;
        }

        public StoredPlot(double[,] zz, string options = "", PlotTypes plotType = PlotTypes.SplotZZ)
        {
            ZZ = zz;
            Options = options;
            PlotType = plotType;
        }

        private bool IsFile(string functionOrFilename)
        {
            int dot = functionOrFilename.LastIndexOf(".");
            if (dot < 1) return false;
            if (char.IsLetter(functionOrFilename[dot - 1]) || char.IsLetter(functionOrFilename[dot + 1]))
                return true;
            return false;
        }
    }
}