﻿using Visitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Application.Models
{
    public class TerritoryModel
    {
    }

    #region Country

    public class Country_Request : BaseEntity
    {
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Country_Response : BaseResponseEntity
    {
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region State

    public class State_Request : BaseEntity
    {
        public string? StateName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class State_Response : BaseResponseEntity
    {
        public string? StateName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class StateDataValidationErrors
    {
        public string StateName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
    public class ImportedState
    {
        public string StateName { get; set; }
        public string IsActive { get; set; }
    }

    #endregion

    #region District

    public class District_Request : BaseEntity
    {
        public string? DistrictName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class District_Response : BaseResponseEntity
    {
        public string? DistrictName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ProvinceDataValidationErrors
    {
        public string ProvinceName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
    public class ImportedProvince
    {
        public string ProvinceName { get; set; }
        public string IsActive { get; set; }
    }

    #endregion

    #region City

    public class City_Request : BaseEntity
    {
        public string? CityName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class City_Response : BaseResponseEntity
    {
        public string? CityName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Area

    public class Area_Request : BaseEntity
    {
        public string? AreaName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Area_Response : BaseResponseEntity
    {
        public string? AreaName { get; set; }

        public bool? IsActive { get; set; }
    }

    #endregion

    #region Territories

    public class Territories_Request : BaseEntity
    {
        public int? CountryId { get; set; }

        public int? StateId { get; set; }

        public int? DistrictId { get; set; }

        public int? CityId { get; set; }

        public int? AreadId { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Territories_Response : BaseResponseEntity
    {
        public int? CountryId { get; set; }

        public string CountryName { get; set; }

        public int? StateId { get; set; }

        public string StateName { get; set; }

        public int? DistrictId { get; set; }

        public string DistrictName { get; set; }

        public int? CityId { get; set; }

        public string CityName { get; set; }

        public int? AreadId { get; set; }

        public string AreaName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class Territories_Country_State_Dist_City_Search
    {
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public int? CityId { get; set; }
    }

    public class Territories_Country_State_Dist_City_Response
    {
        public int? Id { get; set; }

        public string? Value { get; set; }

        public string? Text { get; set; }
    }
    public class TerritoriesDataValidationErrors
    {
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string ProvinceName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
    public class ImportedTerritories
    {
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string ProvinceName { get; set; }
        public string IsActive { get; set; }
    }
    #endregion
}
