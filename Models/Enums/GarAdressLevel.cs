using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum GarAdressLevel
{
    Region, AdministrativeArea, MunicipalArea, RuralUrbanSettlement, City, Locality, ElementOfPlanningStructure,
    ElementOfRoadNetwork, Land, Building, Room, RoomInRooms, AutonomousRegionLevel, IntracityLevel,
    AdditionalTerritoriesLevel, LevelOfObjectsInAdditionalTerritories, CarPlace
}