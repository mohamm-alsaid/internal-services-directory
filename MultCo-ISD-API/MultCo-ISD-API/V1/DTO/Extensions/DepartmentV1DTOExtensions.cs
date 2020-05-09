using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class DepartmentV1DTOExtensions
	{
		public static DepartmentV1DTO ToDepartmentV1DTO(this Department item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			DepartmentV1DTO departmentV1DTO = new DepartmentV1DTO();
			departmentV1DTO.CopyFromDepartment(item);
			return departmentV1DTO;
		}

		public static Department ToDepartment(this DepartmentV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			Department department = new Department();
			department.CopyFromDepartmentV1DTO(item);
			return department;
		}

		public static void CopyFromDepartmentV1DTO(this Department to, DepartmentV1DTO from)
		{
			to.DepartmentId = from.DepartmentId;
			to.DepartmentCode = from.DepartmentCode;
			to.DepartmentName = from.DepartmentName;
		}

		public static void CopyFromDepartment(this DepartmentV1DTO to, Department from)
		{
			to.DepartmentId = from.DepartmentId;
			to.DepartmentCode = from.DepartmentCode;
			to.DepartmentName = from.DepartmentName;
		}
	}
}
