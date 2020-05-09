using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class CommunityV1DTOExtensions
	{
		public static CommunityV1DTO ToCommunityV1DTO(this Community item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			CommunityV1DTO communityV1DTO = new CommunityV1DTO();
			communityV1DTO.CopyFromCommunity(item);
			return communityV1DTO;
		}

		public static Community ToCommunity(this CommunityV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			Community community = new Community();
			community.CopyFromCommunityV1DTO(item);
			return community;
		}

		public static void CopyFromCommunityV1DTO(this Community to, CommunityV1DTO from)
		{
			to.CommunityId = from.CommunityId;
			to.CommunityName = from.CommunityName;
			to.CommunityDescription = from.CommunityDescription;
		}

		public static void CopyFromCommunity(this CommunityV1DTO to, Community from)
		{
			to.CommunityId = from.CommunityId;
			to.CommunityName = from.CommunityName;
			to.CommunityDescription = from.CommunityDescription;
		}
	}
}
