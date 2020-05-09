using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultCo_ISD_API.Models;

namespace MultCo_ISD_API.V1.DTO
{
	public static class LanguageV1DTOExtensions
	{
		public static LanguageV1DTO ToLanguageV1DTO(this Language item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			LanguageV1DTO languageV1DTO = new LanguageV1DTO();
			languageV1DTO.CopyFromLanguage(item);
			return languageV1DTO;
		}

		public static Language ToLanguage(this LanguageV1DTO item)
		{
			if (item == null)
			{
				throw new ArgumentException(nameof(item));
			}

			Language language = new Language();
			language.CopyFromLanguageV1DTO(item);
			return language;
		}

		public static void CopyFromLanguageV1DTO(this Language to, LanguageV1DTO from)
		{
			to.LanguageId = from.LanguageId;
			to.LanguageName = from.LanguageName;
		}

		public static void CopyFromLanguage(this LanguageV1DTO to, Language from)
		{
			to.LanguageId = from.LanguageId;
			to.LanguageName = from.LanguageName;
		}

	}
}
