﻿using GasStationMs.App.Elements;
using GasStationMs.App.Topology.TopologyBuilderHelpers;
using System;
using System.Windows.Forms;

namespace GasStationMs.App.Topology
{
    public partial class TopologyBuilder
    {
        private DataGridView field;
        private int serviceAreaInCells;
        private int serviceAreaBorderColIndex;

        public TopologyBuilder(DataGridView dgv)
        {
            field = dgv ?? throw new NullReferenceException();
            SetupDgv();

            serviceAreaInCells = RecalculateServiceArea();

            SetupServiceArea();
        }

        private void SetupDgv()
        {
            AddDgvCols(Topology.MinColsCount);
            field.RowCount = Topology.MinRowsCount;

            field.RowHeadersVisible = false;
            field.ColumnHeadersVisible = false;

            field.AllowUserToResizeColumns = false;
            field.AllowUserToResizeRows = false;

            field.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            field.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void AddDgvCols(int colsCount)
        {
            for (int i = 0; i < colsCount; i++)
            {
                field.Columns.Add(new BlankTopologyBuilderCol());
            }
        }

        private void AddDgvRows(int rowsCount)
        {
            for (int i = 0; i < rowsCount; i++)
            {
                field.Rows.Add();
            }
        }

        public void SetupServiceArea()
        {
            int сellsLeftToAdd = serviceAreaInCells;
            int cellsAdded = 0;

            DataGridViewImageCell cell;

            for (int currCol = field.ColumnCount - 1; currCol >= 0; currCol--)
            {
                for (int currRow = 0; currRow < field.RowCount; currRow++)
                {
                    cell = (DataGridViewImageCell)field.Rows[currRow].Cells[currCol];
                    cell.Tag = new ServiceArea();
                    cell.Value = ServiceArea.Image;

                    cellsAdded++;
                    сellsLeftToAdd--;
                }

                if (сellsLeftToAdd <= 0)
                {
                    serviceAreaInCells = cellsAdded;
                    serviceAreaBorderColIndex = currCol;
                    break;
                }
            }
        }

        public int ColsCount
        {
            get
            {
                return field.ColumnCount;
            }

            set
            {
                if (value < Topology.MinColsCount)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (value > Topology.MaxColsCount)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (field.ColumnCount < value)
                {
                    AddDgvCols(value - field.ColumnCount);
                }
                else
                {
                    RemoveDgvCols(field.ColumnCount - value);
                }

                field.ColumnCount = value;

                //serviceAreaInCells = RecalculateServiceArea();
            }
        }

        public int RowsCount
        {
            get
            {
                return field.RowCount;
            }

            set
            {
                if (value < Topology.MinRowsCount)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (value > Topology.MaxRowsCount)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (field.RowCount < value)
                {
                    AddDgvRows(value - field.RowCount);
                }
                else
                {
                    RemoveDgvRows(field.RowCount - value);
                }

                field.RowCount = value;

                //serviceAreaInCells = RecalculateServiceArea();
            }
        }

        private void RemoveDgvCols(int colsCount)
        {
            for (int i = 0; i < colsCount; i++)
            {
                DataGridViewColumn dgvCol = field.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None);

                if (IsThereAnyTag(dgvCol))
                {
                    throw new CannotRemoveTopologyBuilderCol();
                }

                field.Columns.Remove(dgvCol);
            }
        }

        private void RemoveDgvRows(int rowsCount)
        {
            for (int i = 0; i < rowsCount; i++)
            {
                int rowIndex = field.Rows.GetLastRow(DataGridViewElementStates.Visible);
                DataGridViewRow row = field.Rows[rowIndex];

                if (IsThereAnyTag(row))
                {
                    throw new CannotRemoveTopologyBuilderRow();
                }

                try
                {
                    DataGridViewImageCell[] penultimateRowCells = new DataGridViewImageCell[field.ColumnCount];
                    int penultimateRowIndex = rowIndex - 1;
                    DataGridViewImageCell cell;
                    for (int penultimateColIndex = 0; penultimateColIndex < penultimateRowCells.Length; penultimateColIndex++)
                    {
                        cell = (DataGridViewImageCell)field.Rows[penultimateRowIndex].Cells[penultimateColIndex];
                        penultimateRowCells[penultimateColIndex].Tag = cell.Tag;
                        penultimateRowCells[penultimateColIndex].Value = cell.Value;
                    }

                    field.Rows.Remove(row);

                    rowIndex = field.Rows.GetLastRow(DataGridViewElementStates.Visible);
                    for (int colIndex = 0; colIndex < field.ColumnCount; colIndex++)
                    {
                        cell = (DataGridViewImageCell)field.Rows[rowIndex].Cells[colIndex];
                        cell.Tag = penultimateRowCells[colIndex].Tag;
                        cell.Value = penultimateRowCells[colIndex].Value;
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private bool IsThereAnyTag(DataGridViewColumn col)
        {
            int colIndex = col.Index;
            DataGridViewImageCell cell;

            for (int rowIndex = 0; rowIndex < field.RowCount; rowIndex++)
            {
                cell = (DataGridViewImageCell)field.Rows[rowIndex].Cells[colIndex];

                if (cell.Tag != null)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsThereAnyTag(DataGridViewRow row)
        {
            int rowIndex = row.Index;
            DataGridViewImageCell cell;

            for (int colIndex = 0; colIndex < field.ColumnCount; colIndex++)
            {
                cell = (DataGridViewImageCell)field.Rows[rowIndex].Cells[colIndex];

                if (cell.Tag != null)
                {
                    return true;
                }
            }

            return false;
        }

        private int RecalculateServiceArea()
        {
            return (int)(RowsCount * ColsCount * Topology.ServiceAreaInShares);
        }

        public Topology CreateAndGetTopology()
        {
            IGasStationElement[,] gseArr = new IGasStationElement[field.RowCount, field.ColumnCount];

            DataGridViewImageCell cell;
            for (int currRow = 0; currRow < gseArr.GetLength(0); currRow++)
            {
                for (int currCol = 0; currCol < gseArr.GetLength(1); currCol++)
                {
                    cell = (DataGridViewImageCell)field.Rows[currRow].Cells[currCol];
                    if (cell.Tag != null)
                    {
                        gseArr[currRow, currCol] = (IGasStationElement)cell.Tag;
                    }
                    else
                    {
                        gseArr[currRow, currCol] = null;
                    }
                }
            }

            return new Topology(gseArr, serviceAreaBorderColIndex);
        }
    }
}