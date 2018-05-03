using Lab8.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab8.Services
{
    public class InputModelResizer
    {
        /// <summary>
        /// Returns resized inputModel object
        /// </summary>
        public InputModel Resize(InputModel inputModel, int newWidth, int newHeight)
        {
            var matrix = new double?[newHeight][];
            for(int i = 0; i < newHeight; i++)
                matrix[i] = new double?[newWidth];

            var minH = Math.Min(newHeight, inputModel.MatrixHeight);
            var minW = Math.Min(newWidth, inputModel.MatrixWidth);
            for (int i = 0; i < minH; i++)
                for (int j = 0; j < minW; j++)
                    matrix[i][j] = inputModel.Matrix[i][j];

            return new InputModel
            {
                MatrixHeight = newHeight,
                MatrixWidth = newWidth,
                MatrixHeightUpdated = inputModel.MatrixHeightUpdated,
                MatrixWidthUpdated = inputModel.MatrixWidthUpdated,
                Matrix = matrix
            };
        }
    }
}