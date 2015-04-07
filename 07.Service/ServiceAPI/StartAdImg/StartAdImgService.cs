using IplusOAEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAPI
{
    public class StartAdImgService
    {

        public static StartAdImgEntity GetAppStateIndex(string depId, List<DepartmentEntity> depList, List<StartAdImgEntity> imgList)
        {

            StartAdImgEntity resStartAdImgEntity = null;
            DepartmentEntity depEntity = depList.FirstOrDefault(x => x.Id == depId);
            StartAdImgEntity startAdImgEntity = imgList.FirstOrDefault(x => x.DepId == depId && x.State == 0);

            if (startAdImgEntity != null)//直接找到该社区有效广告图
            {
                resStartAdImgEntity = startAdImgEntity;
                return resStartAdImgEntity;
            }



            while (depEntity != null)
            {
                string tmpDepId;
                if (startAdImgEntity == null)
                {
                    tmpDepId = depId;
                }
                else
                {
                    tmpDepId = startAdImgEntity.DepId;
                }
                depEntity = depList.FirstOrDefault(x => x.Id == tmpDepId);

                StartAdImgEntity tmp = imgList.FirstOrDefault(x => x.DepId == depEntity.PId);

                if (tmp != null)
                {
                    startAdImgEntity = tmp;
                    if (tmp.State == 0)
                    {
                        resStartAdImgEntity = tmp;
                    }
                }
                else
                {
                    break;
                }
            }

            return resStartAdImgEntity;
        }
    }
}
