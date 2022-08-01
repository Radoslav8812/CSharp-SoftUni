using System;
namespace SoftJail.Common
{
    public class ValidationConstants
    {
        public ValidationConstants()
        {
        }

        public const int PrisonerFullNameMinLength = 3;
        public const int PrisonerFullNameMaxLength = 20;

        public const int OfficerFullNameMinLength = 3;
        public const int OfficerFullNameMaxLength = 30;

        public const int DepartmentNameMinLength = 3;
        public const int DepartmentNameMaxLength = 25;

        public const int CellNumberMinValue = 1;
        public const int CellNumberMaxValue = 1000;
    }
}

