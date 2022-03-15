using AutoMapper;
using DBService.Entities;
using Dtos.Common;
using Dtos.Enums;
using Dtos.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationServices.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            #region Transactions
            CreateMap<NewTransactionRequest, Transactions>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(y => DateTime.Now));

            CreateMap<Transactions, Transact>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(a => ((EnumStatus)a.StatusId).ToString() ));

            CreateMap<Transactions, TransactHistoric>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(a => a.Status.Description))
                .ForMember(dest => dest.Historics, opt => opt.MapFrom(MapTransactionHistoricMin));

            #endregion

            #region Catalogs

            #endregion

            #region RolesMaps
            //CreateMap<Roles, RoleDto>();
            #endregion

            #region CollaboratorsMaps
            //CreateMap<NewCollaboratorRequest, Collaborators>().ForMember(
            //    collaborator => collaborator.CollaboratorsTechnologies,
            //    member => member.MapFrom(MapCollaboratorTechnologiesRequest)
            //);


            #endregion

            #region ApiMaps

            //CreateMap<OpenWatherResponse, WeatherInfoDto>()
            //    .ForMember(destino => destino.Weather, opt => opt.MapFrom(x => x.current.weather[0].description))
            //    .ForMember(destino => destino.Currtemp, opt => opt.MapFrom(y => KelvinToCelcius(y.current.temp)))
            //    .ForMember(destino => destino.Maxtemp, opt => opt.MapFrom(z => KelvinToCelcius(z.daily[0].temp.max)))
            //    .ForMember(destino => destino.Mintemp, opt => opt.MapFrom(a => KelvinToCelcius(a.daily[0].temp.min)));
            #endregion

        }

        #region Methods
        private List<Historic> MapTransactionHistoricMin(Transactions transaction, TransactHistoric data)
        {
            var result = new List<Historic>();
            if (transaction.Historic == null || transaction.Historic?.Count == 0)
            {
                return result;
            }

            foreach (var item in transaction.Historic)
            {
                result.Add(new Historic
                {
                    Comment = item.Comment,
                    Date = item.Date,
                    Id = item.Id,
                    NewStatus = ((EnumStatus)item.NewStatusId).ToString(),
                    OldStatus = ((EnumStatus)item.OldStatusId).ToString()
                });
            }

            return result;
        }
        #endregion

    }
}
