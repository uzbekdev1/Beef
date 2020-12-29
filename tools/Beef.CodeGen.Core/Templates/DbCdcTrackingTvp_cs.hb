﻿{{! Copyright (c) Avanade. Licensed under the MIT License. See https://github.com/Avanade/Beef }}
/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable

using System.Collections.Generic;
using System.Data;
using Beef.Data.Database;
using Beef.Data.Database.Cdc;

namespace {{Root.Company}}.{{Root.AppName}}.Cdc.Data
{
    /// <summary>
    /// Provides the <see cref="CdcTracker"/> data access.
    /// </summary>
    public class CdcTrackingDbMapper : DatabaseMapper<CdcTracker>, ITrackingTvp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdcTrackingDbMapper"/> class.
        /// </summary>
        public CdcTrackingDbMapper()
        {
            Property(s => s.Key);
            Property(s => s.Hash);
        }

        /// <summary>
        /// Creates a <see cref="TableValuedParameter"/> for the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">The <see cref="CdcTracker"/> list.</param>
        /// <returns>The Table-Valued Parameter.</returns>
        public TableValuedParameter CreateTableValuedParameter(IEnumerable<CdcTracker> list)
        {        
            var dt = new DataTable();
            dt.Columns.Add("Key", typeof(string));
            dt.Columns.Add("Hash", typeof(string));

            var tvp = new TableValuedParameter("[{{Root.CdcSchema}}].[udt{{Root.CdcTrackingTableName}}List]", dt);
            AddToTableValuedParameter(tvp, list);
            return tvp;
        }
    }
}

#nullable restore