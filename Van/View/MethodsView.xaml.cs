﻿using System;
using System.Windows.Controls;
using static SharedLibrary.Helper.HelperMethods;


namespace Van.View.Methods
{
    public partial class MethodsView : UserControl, IDisposable
    {
        public MethodsView()
        {
            InitializeComponent();
            MortalityTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            SurvivalFunctionTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            LifeTimesTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            QualityAssessmentOfModelsTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            DensityTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            ResidualSurvivalFunctionTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            ResidualDensityTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            DistanceFirstMethodTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            DistanceSecondMethodTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
        }

        public void Dispose()
        {
            MortalityTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            SurvivalFunctionTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            LifeTimesTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            QualityAssessmentOfModelsTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            DensityTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            ResidualSurvivalFunctionTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            ResidualDensityTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            DistanceFirstMethodTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            DistanceSecondMethodTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
        }
    }
}
