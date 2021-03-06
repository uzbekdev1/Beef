/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable
#pragma warning disable IDE0005 // Using directive is unnecessary; are required depending on code-gen options

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beef;
using Beef.Business;
using Beef.Data.Database;
using Beef.Entities;
using Beef.Mapper;
using Beef.Mapper.Converters;
using Beef.Demo.Common.Entities;
using RefDataNamespace = Beef.Demo.Common.Entities;

namespace Beef.Demo.Business.Data
{
    /// <summary>
    /// Provides the <see cref="Gender"/> data access.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Will not always appear static depending on code-gen options")]
    public partial class GenderData : IGenderData
    {
        private readonly IDatabase _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderData"/> class.
        /// </summary>
        /// <param name="db">The <see cref="IDatabase"/>.</param>
        public GenderData(IDatabase db)
            { _db = Check.NotNull(db, nameof(db)); GenderDataCtor(); }

        partial void GenderDataCtor(); // Enables additional functionality to be added to the constructor.

        /// <summary>
        /// Gets the specified <see cref="Gender"/>.
        /// </summary>
        /// <param name="id">The <see cref="Gender"/> identifier.</param>
        /// <returns>The selected <see cref="Gender"/> where found.</returns>
        public Task<Gender?> GetAsync(Guid id)
        {
            return DataInvoker.Current.InvokeAsync(this, async () =>
            {
                var __dataArgs = DbMapper.Default.CreateArgs("[Ref].[spGenderGet]");
                return await _db.GetAsync(__dataArgs, id).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// Creates a new <see cref="Gender"/>.
        /// </summary>
        /// <param name="value">The <see cref="Gender"/>.</param>
        /// <returns>The created <see cref="Gender"/>.</returns>
        public Task<Gender> CreateAsync(Gender value)
        {
            return DataInvoker.Current.InvokeAsync(this, async () =>
            {
                var __dataArgs = DbMapper.Default.CreateArgs("[Ref].[spGenderCreate]");
                return await _db.CreateAsync(__dataArgs, Check.NotNull(value, nameof(value))).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// Updates an existing <see cref="Gender"/>.
        /// </summary>
        /// <param name="value">The <see cref="Gender"/>.</param>
        /// <returns>The updated <see cref="Gender"/>.</returns>
        public Task<Gender> UpdateAsync(Gender value)
        {
            return DataInvoker.Current.InvokeAsync(this, async () =>
            {
                var __dataArgs = DbMapper.Default.CreateArgs("[Ref].[spGenderUpdate]");
                return await _db.UpdateAsync(__dataArgs, Check.NotNull(value, nameof(value))).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// Provides the <see cref="Gender"/> property and database column mapping.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design; as there is a direct relationship")]
        public partial class DbMapper : DatabaseMapper<Gender, DbMapper>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DbMapper"/> class.
            /// </summary>
            public DbMapper()
            {
                Property(s => s.Id, "GenderId").SetUniqueKey(true);
                Property(s => s.Code);
                Property(s => s.Text);
                Property(s => s.IsActive);
                Property(s => s.SortOrder);
                Property(s => s.AlternateName);
                Property(s => s.TripCode);
                AddStandardProperties();
                DbMapperCtor();
            }
            
            partial void DbMapperCtor(); // Enables the DbMapper constructor to be extended.
        }
    }
}

#pragma warning restore IDE0005
#nullable restore