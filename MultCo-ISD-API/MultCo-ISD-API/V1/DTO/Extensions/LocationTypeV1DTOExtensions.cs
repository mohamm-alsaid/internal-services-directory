using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
    public static class LocationTypeV1DTOExtensions
    {


		public static LocationTypeV1DTO ToLocationTypeV1DTO(this LocationType item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			LocationTypeV1DTO locationTypeV1DTO = new LocationTypeV1DTO();
			locationTypeV1DTO.CopyFromLocationType(item);
			return locationTypeV1DTO;
		}

		public static LocationType ToLocationType(this LocationTypeV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			LocationType locationType = new LocationType();
			locationType.CopyFromLocationTypeV1DTO(item);
			return locationType;
		}

		public static void CopyFromLocationTypeV1DTO(this LocationType to, LocationTypeV1DTO from)
		{
			to.LocationTypeId = from.LocationTypeId;
			to.LocationTypeName = from.LocationTypeName;
		}

		public static void CopyFromLocationType(this LocationTypeV1DTO to, LocationType from)
		{
			to.LocationTypeId = from.LocationTypeId;
			to.LocationTypeName = from.LocationTypeName;
		}

	}
}
