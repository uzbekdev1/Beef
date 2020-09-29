﻿// Copyright (c) Avanade. Licensed under the MIT License. See https://github.com/Avanade/Beef

using Beef.CodeGen.Entities;
using Newtonsoft.Json;
using System.Text;

namespace Beef.CodeGen.Config.Database
{
    /// <summary>
    /// Represents the column configuration.
    /// </summary>
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [ClassSchema("Parameter", Title = "The **Where** statement is used to define additional filtering.", Description = "", Markdown = "")]
    [CategorySchema("Key", Title = "Provides the **key** configuration.")]
    public class ColumnConfig : ConfigBase<CodeGenConfig, TableConfig>
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the database <see cref="Column"/> configuration.
        /// </summary>
        public Column? DbColumn { get; set; }

        /// <summary>
        /// Gets the qualified name (includes the alias).
        /// </summary>
        public string QualifiedName => $"[{Parent!.Alias}].[{Name}]";

        /// <summary>
        /// Gets the parameter name.
        /// </summary>
        public string ParameterName => "@" + Name;

        /// <summary>
        /// Gets the SQL type.
        /// </summary>
        public string? SqlType { get; private set; }

        /// <summary>
        /// Gets the parameter SQL definition.
        /// </summary>
        public string? ParameterSql { get; private set; }

        /// <summary>
        /// Gets the UDT SQL definition.
        /// </summary>
        public string? UdtSql { get; private set; }

        /// <summary>
        /// Gets the where equality clause.
        /// </summary>
        public string WhereEquals => Name == Parent?.ColumnIsDeleted?.Name ? $"ISNULL({QualifiedName}, 0) = 0" : $"{QualifiedName} = {ParameterName}";

        /// <summary>
        /// Gets the SQL for defining initial value for comparisons.
        /// </summary>
        public string SqlInitialValue => DbColumn!.Type!.ToUpperInvariant() == "UNIQUEIDENTIFIER"
            ? "CONVERT(UNIQUEIDENTIFIER, '00000000-0000-0000-0000-000000000000')"
            : (Column.TypeIsInteger(DbColumn!.Type) || Column.TypeIsDecimal(DbColumn!.Type) ? "0" : "''");

        /// <summary>
        /// Indicates whether the column is considered an audit column.
        /// </summary>
        public bool IsAudit => IsCreated || IsUpdated || IsDeleted;

        /// <summary>
        /// Indicates whether the column is "CreatedBy" or "CreatedDate".
        /// </summary>
        public bool IsCreated => IsCreatedBy || IsCreatedDate;

        /// <summary>
        /// Indicates whether the column is "CreatedBy".
        /// </summary>
        public bool IsCreatedBy => Name == Parent!.ColumnCreatedBy?.Name;

        /// <summary>
        /// Indicates whether the column is "CreatedDate".
        /// </summary>
        public bool IsCreatedDate => Name == Parent!.ColumnCreatedDate?.Name;

        /// <summary>
        /// Indicates whether the column is "UpdatedBy" or "UpdatedDate".
        /// </summary>
        public bool IsUpdated => IsUpdatedBy || IsUpdatedDate;

        /// <summary>
        /// Indicates whether the column is "UpdatedBy".
        /// </summary>
        public bool IsUpdatedBy => Name == Parent!.ColumnUpdatedBy?.Name;

        /// <summary>
        /// Indicates whether the column is "UpdatedDate".
        /// </summary>
        public bool IsUpdatedDate => Name == Parent!.ColumnUpdatedDate?.Name;

        /// <summary>
        /// Indicates whether the column is "DeletedBy" or "DeletedDate".
        /// </summary>
        public bool IsDeleted => IsDeletedBy || IsDeletedDate;

        /// <summary>
        /// Indicates whether the column is "DeletedBy".
        /// </summary>
        public bool IsDeletedBy => Name == Parent!.ColumnDeletedBy?.Name;

        /// <summary>
        /// Indicates whether the column is "DeletedDate".
        /// </summary>
        public bool IsDeletedDate => Name == Parent!.ColumnDeletedDate?.Name;

        /// <summary>
        /// Indicates where the column should be considered for a 'Create' operation.
        /// </summary>
        public bool IsCreateColumn => (!DbColumn!.IsComputed && !IsAudit) || IsCreated;

        /// <summary>
        /// Indicates where the column should be considered for a 'Update' operation.
        /// </summary>
        public bool IsUpdateColumn => (!DbColumn!.IsComputed && !IsAudit) || IsUpdated;

        /// <summary>
        /// Indicates where the column should be considered for a 'Delete' operation.
        /// </summary>
        public bool IsDeleteColumn => (!DbColumn!.IsComputed && !IsAudit) || IsDeleted;

        /// <summary>
        /// Indicates where the column is the "TenantId" column.
        /// </summary>
        public bool IsTenantIdColumn => Name == Parent!.ColumnTenantId?.Name;

        /// <summary>
        /// Indicates where the column is the "OrgUnitId" column.
        /// </summary>
        public bool IsOrgUnitIdColumn => Name == Parent!.ColumnOrgUnitId?.Name;

        /// <summary>
        /// Indicates where the column is the "RowVersion" column.
        /// </summary>
        public bool IsRowVersionColumn => Name == Parent!.ColumnRowVersion?.Name;

        /// <summary>
        /// Indicates where the column is the "IsDeleted" column.
        /// </summary>
        public bool IsIsDeletedColumn => Name == Parent!.ColumnIsDeleted?.Name;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void Prepare()
        {
            UpdateSqlProperties();
        }

        /// <summary>
        /// Update the required SQL properties.
        /// </summary>
        private void UpdateSqlProperties()
        {
            var sb = new StringBuilder(DbColumn!.Type!.ToUpperInvariant());
            if (Column.TypeIsString(DbColumn!.Type))
                sb.Append(DbColumn!.Length.HasValue && DbColumn!.Length.Value > 0 ? $"({DbColumn!.Length.Value})" : "(MAX)");

            sb.Append(DbColumn!.Type.ToUpperInvariant() switch
            {
                "DECIMAL" => $"({DbColumn!.Precision}, {DbColumn!.Scale})",
                "NUMERIC" => $"({DbColumn!.Precision}, {DbColumn!.Scale})",
                "TIME" => DbColumn!.Scale.HasValue && DbColumn!.Scale.Value > 0 ? $"({DbColumn!.Scale})" : string.Empty,
                _ => string.Empty
            });

            if (DbColumn!.IsNullable)
                sb.Append(" NULL");

            SqlType = sb.ToString();
            ParameterSql = $"{ParameterName} AS {SqlType}";
            UdtSql = $"[{Name}] {SqlType}";
        }
    }
}