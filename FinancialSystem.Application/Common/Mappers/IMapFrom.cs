using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSystem.Application.Common.Mappers
{
    public interface IMapFrom<T>
    {
        // Метод по умолчанию: если класс реализует этот интерфейс, 
        // он автоматически маппится из типа T в текущий класс.
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
