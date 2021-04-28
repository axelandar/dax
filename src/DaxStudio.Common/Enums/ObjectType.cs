﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaxStudio.Common.Enums
{
    // codes taken from: https://docs.microsoft.com/en-us/analysis-services/trace-events/analysis-services-object-type-codes-used-in-traces?view=asallproducts-allversions
    class ot
    {
        static ot()
        {
            Dictionary<long,string> objTypes = new Dictionary<long, string>();

        }
    }
    
    public enum ObjectType
    {

        //TABULAR OBJECT TYPES
        Database = 801002,
        Model = 802010,
        Datasource = 802011,
        Table = 802012,
        Column = 802013,
        AttributeHierarchy = 802014,
        Partition = 802015,
        Relationship = 802016,
        Measure = 802017,
        Hierarchy = 802018,
        Level = 802019,
        Annotation = 802020,
        KPI = 802021,
        Culture = 802022,
        ObjectTranslation = 802023,
        LinguisticMetadata = 802024,
        Perspective = 802038,
        PerspectiveTable = 802039,
        PerspectiveTableColumn = 802040,
        PerspectiveTableHierarchy = 802041,
        PerspectiveTableMeasure = 802042,
        Role = 802043,
        RoleMembership = 802044,
        TablePermission = 802045,
        Variation = 802046,
        ColumnPermission = 802051,
        DetailRowsDefinition = 802052,
        AlternateOf = 802055,

        // MULTIDIMENSIONAL OBJECT TYPES
        Server = 100000,
        MdCommand = 100001,
        MdDatabase = 100002,
        MdDataSource = 100003,
        MdDatabasePermission = 100004,
        MdRole = 100005,
        MdDimension = 100006,
        MdDimensionAttribute = 100007,
        MdHierarchy = 100008,
        MdLevel = 100009,
        MdCube = 100010,
        MdCubePermission = 100011,
        MdCubeDimension = 100012,
        MdCubeAttribute = 100013,
        MdCubeHierarchy = 100014,
        MdMeasureGroup = 100016,
        MdMeasureGroupDimension = 100017,
        MdMeasureGroupAttribute = 100018,
        MdMeasure = 100020,
        MdPartition = 100021,
        MdAggregationDesign = 100025,
        MdAggregationDesignDimension = 100026,
        MdAggregationDesignAttribute = 100027,
        MdAggregation = 100028,
        MdAggregationDimension = 100029,
        MdAggregationAttribute = 100030,
        MdMiningStructure = 100032,
        MdMiningStructureColumn = 100033,
        MdMiningModel = 100037,
        MdMiningModelColumn = 100038,
        MdAlgorithmParameter = 100039,
        MdMiningModelPermission = 100041,
        MdDimensionPermission = 100042,
        MdMiningStructurePermission = 100043,
        MdAssembly = 100044,
        MdDatabaseRole = 100045,
        MdAttributePermission = 100046,
        MdCubeAttributePermission = 100047,
        MdCellPermission = 100048,
        MdCubeDimensionPermission = 100049,
        MdTrace = 100050,
        MdServerAssembly = 100051,
        MdCubeAssembly = 100052,
        MdCommand2 = 100053,
        MdKPI = 100054,
        MdDataSourceView = 100055,
        MdPerspective = 100056,
        MdCommandCollection2 = 100100,
        MdDatabaseCollection = 100101,
        MdDataSourceCollection = 100102,
        MdDatabasePermission2 = 100103,
        MdRoleCollection = 100104,
        MdDimensionCollection = 100105,
        MdDimensionAttributeCollection = 100106,
        MdHierarchyCollection = 100107,
        MdLevelCollection = 100108,
        MdCubeCollection = 100109,
        MdCubePermissionCollection = 100110,
        MdCubeDimensionCollection = 100111,
        MdCubeAttributeCollection = 100112,
        MdCubeHierarchyCollection = 100113,
        MdMeasureGroupCollection = 100115,
        MdMeasureGroupDimensionCollection = 100116,
        MdMeasureGroupAttributeCollection = 100117,
        MdMeasureCollection = 100119,
        MdPartitionCollection = 100120,
        MdAggregationDesignCollection = 100124,
        MdAggregationDesignDimensionCollection = 100125,
        MdAggregationDesignAttributeCollection = 100126,
        MdAggregationCollection = 100127,
        MdAggregationDimensionCollection = 100128,
        MdAggregationAttributeCollection = 100129,
        MdMiningStructureCollection = 100131,
        MdMiningStructureColumnCollection = 100132,
        MdMiningModelCollection = 100136,
        MdMiningModelColumnCollection = 100137,
        MdAlgorithmParameterCollection = 100138,
        MdMiningModelPermissionCollection = 100140,
        MdDimensionPermissionCollection = 100141,
        MdMiningStructurePermissionCollection = 100142,
        MdAssemblyCollection = 100143,
        MdDatabaseRoleCollection = 100144,
        MdAttributePermissionCollection = 100145,
        MdCubeAttributePermissionCollection = 100146,
        MdCellPermissionCollection = 100147,
        MdCubeDimensionPermissionCollection = 100148,
        MdTraceCollection = 100149,
        MdServerAssemblyCollection = 100150,
        MdCubeAssemblyCollection = 100151,
        MdCommandCollection = 100152,
        MdKpiCollection = 100153,
        MdDataSourceViewCollection = 100154,
        MdPerspectiveCollection = 100155,


    }
}