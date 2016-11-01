using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoConShop.Business.Dtos;
using ThoConShop.DAL.Entities;

namespace ThoConShop.Business.Contracts
{
    public interface IAccountRelationDataService
    {
        IList<RankDto> ReadRankForFilter();

        IList<SkinDto> ReadSkinForFilter();

        IList<string> ReadPriceRangeForFilter();
    }
}
