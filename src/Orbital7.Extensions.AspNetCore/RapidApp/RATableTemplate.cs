using System.Diagnostics.CodeAnalysis;

namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableTemplate
{
    public const string INDEX_COLUMN_HEADER = "#";

    public static RATableTemplate<NamedValue<object>> CreateForNamedValue(
        string namesHeader = "Property",
        bool sortByNames = true,
        bool isNamesSortable = true,
        bool isValuesSortable = true)
    {
        return new RATableTemplate<NamedValue<object>>(
            [
                new(
                    x => x.Name,
                    headerText: namesHeader,
                    sortBy: sortByNames,
                    isSortable: isNamesSortable),
                new(
                    x => x.Value,
                    isSortable: isValuesSortable,
                    cellHorizontalAlignment: RATableViewCellHorizontalAlignment.Right,
                    headerCellHorizontalAlignment: RATableViewCellHorizontalAlignment.Right),
            ]);
    }

    public static RATableTemplate<PropertyValue> CreateForPropertyValue(
        string namesHeader = "Property",
        bool sortByNames = true,
        bool isNamesSortable = true,
        bool isValuesSortable = true)
    {
        return new RATableTemplate<PropertyValue>(
            [
                new(
                    x => x.DisplayName,
                    headerText: namesHeader,
                    sortBy: isNamesSortable && sortByNames,
                    isSortable: isNamesSortable),
                new(
                    x => x.Value,
                    sortBy: !sortByNames && isValuesSortable,
                    isSortable: isValuesSortable,
                    cellHorizontalAlignment: RATableViewCellHorizontalAlignment.Right,
                    headerCellHorizontalAlignment: RATableViewCellHorizontalAlignment.Right),
            ],
            isSortable: isNamesSortable || isValuesSortable);
    }
}

public class RATableTemplate<TEntity> :
    RATableTemplate
{
    public bool? IsSortableOverride { get; init; }

    public List<Column<TEntity>> Columns { get; private set; }

    public Action<RATableViewSegment<TEntity>, TEntity>? OnRowSelected { get; init; }

    public bool HasFooter => this.Columns.Count > 0 &&
        (this.Columns.Any(x => x.GetFooterCellValue != null) ||
         this.Columns.Any(x => x.GetFooterCellContent != null));

    public bool IsSortable => 
        ((!this.IsSortableOverride.HasValue || this.IsSortableOverride.Value) && HasSortableColumn()) ||
         (this.IsSortableOverride.HasValue && !this.IsSortableOverride.Value && HasSortableOverrideColumn());

    public RATableTemplate(
        List<Column<TEntity>> columns,
        Action<RATableViewSegment<TEntity>, TEntity>? onRowSelected = null,
        bool? isSortable = null)
    {
        this.Columns = columns;
        this.OnRowSelected = onRowSelected;

        this.IsSortableOverride = isSortable;
    }

    public IDictionary<int, TEntity>? GetSortedItems(
        RATableViewSegment<TEntity> segment)
    {
        if (segment.Items != null)
        {
            if (this.IsSortable)
            {
                var sortColumn = GetCurrentSortColumn();

                // If there's no currently specified sort column, use the first
                // sortable column.
                if (sortColumn == null)
                {
                    // Look for a sortable index column first.
                    sortColumn = this.Columns
                        .Where(x =>
                            x.IsSortable &&
                            x.IsIndexColumn())
                        .FirstOrDefault();

                    // If still not found, then find the first sortable
                    // column with a For expression.
                    if (sortColumn == null)
                    {
                        sortColumn = this.Columns
                            .Where(x =>
                                x.IsSortable &&
                                x.For?.Body != null)
                            .FirstOrDefault();
                    }
                }

                // Sort if we have a column to sort by.
                if (sortColumn != null)
                {
                    // Check for a custom sort function.
                    if (sortColumn.GetCellSortValue != null)
                    {
                        return ExecuteItemsSort(
                            segment,
                            x => sortColumn.GetCellSortValue(sortColumn, segment, x.Value),
                            sortColumn.SortDescending);
                    }
                    // Else check for the index column.
                    else if (sortColumn.IsIndexColumn())
                    {
                        return ExecuteItemsSort(
                            segment,
                            x => x.Key,
                            sortColumn.SortDescending);
                    }
                    // Else proceed only if we have a For expression.
                    else if (sortColumn.For != null)
                    {
                        // Compile the For statement.
                        var compiledFor = sortColumn.For.Compile();

                        // Determine the property type so we handle based on type.
                        var memberInfo = sortColumn.For.Body?.GetMemberInfo();
                        var propertyType = (memberInfo as PropertyInfo)?.PropertyType;

                        // For Enums, sort by Display String value (the default is integer value sorting).
                        if (propertyType?.IsBaseOrNullableEnumType() ?? false)
                        {
                            return ExecuteItemsSort(
                                segment,
                                x => (compiledFor(x.Value) as Enum)?.ToDisplayString() ?? String.Empty,
                                sortColumn.SortDescending);
                        }
                        // Use default sorting.
                        else
                        {
                            return ExecuteItemsSort(
                                segment,
                                x => compiledFor(x.Value),
                                sortColumn.SortDescending);
                        }
                    }
                }
            }
            
            return segment.Items;
        }

        return null;
    }

    private IDictionary<int, TEntity> ExecuteItemsSort(
        RATableViewSegment<TEntity> segment,
        Func<KeyValuePair<int, TEntity>, object?> keySelector,
        bool descending)
    {
        return descending ?
                segment.Items.OrderByDescending(keySelector).ToDictionary(x => x.Key, x => x.Value) :
                segment.Items.OrderBy(keySelector).ToDictionary(x => x.Key, x => x.Value);
    }

    internal void SetSortByColumn(
        Column<TEntity> column,
        bool sortDescending)
    {
        // Unselect current sort column.
        var currentSortColumn = GetCurrentSortColumn();
        if (currentSortColumn != null)
        {
            currentSortColumn.SortBy = false;
            currentSortColumn.SortDescending = false;
        }

        // Select the new column.
        column.SortBy = true;
        column.SortDescending = sortDescending;
    }

    private Column<TEntity>? GetCurrentSortColumn()
    {
        return this.Columns
            .Where(x => 
                x.SortBy && 
                x.IsSortable)
            .FirstOrDefault();
    }

    private bool HasSortableColumn()
    {
        return this.Columns.Any(x => x.IsSortable);
    }

    private bool HasSortableOverrideColumn()
    {
        return this.Columns.Any(x => x.IsSortableOverride.HasValue && x.IsSortableOverride.Value);
    }

    public class Column<TItem>
    {
        private Func<TItem, object?>? _compiledFor;
        private MemberInfo? _memberInfo;

        public string? HeaderText { get; init; }

        internal bool? IsSortableOverride { get; init; }

        public bool SortDescending { get; internal set; } = false;

        public bool SortBy { get; internal set; } = false;

        public string? CellClass { get; init; }

        public int? FixedWidth { get; init; }

        public Expression<Func<TItem, object?>>? For { get; set; }

        public Action<Column<TItem>, RATableViewSegment<TItem>, TItem>? OnCellUrlClicked { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, string>? GetCellUrl { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, string>? GetCellClass { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, string>? GetCellStyle { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, object>? GetCellValue { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, object>? GetCellSortValue { get; set; }

        public RenderFragment<TItem>? GetCellContent { get; set; }

        public RenderFragment<List<RATableViewSegment<TEntity>>>? GetHeaderCellContent { get; set; }

        public RenderFragment<RATableViewFooterData<TItem>>? GetFooterCellContent { get; set; }

        public Func<RATableViewFooterData<TItem>, object>? GetFooterCellValue { get; set; }

        public Func<RATableViewFooterData<TItem>, string>? GetFooterCellClass { get; set; }

        public DisplayValueOptions? DisplayValueOptions { get; set; }

        public Action<Column<TItem>, DisplayValueOptions>? ConfigureDisplayValueOptions { get; set; }

        public string? DefaultValue { get; set; } = "-";

        public RATableViewCellHorizontalAlignment CellHorizontalAlignment { get; set; }

        public RATableViewCellHorizontalAlignment HeaderCellHorizontalAlignment { get; set; }

        public RATableViewCellHorizontalAlignment? FooterCellHorizontalAlignment { get; set; }

        public bool IsSortable => 
            this.For?.Body != null || 
            this.GetCellSortValue != null || 
            IsIndexColumn();

        public Column(
            Expression<Func<TItem, object?>>? forValue,
            string? headerText = null,
            bool? sortBy = null,
            bool? isSortable = null,
            bool? sortDescending = null,
            RATableViewCellHorizontalAlignment? cellHorizontalAlignment = null,
            RATableViewCellHorizontalAlignment? headerCellHorizontalAlignment = null,
            string? cellClass = null)
        {
            _memberInfo = forValue?.Body.GetMemberInfo();

            if (forValue != null)
            {
                this.For = forValue;
            }

            if (this.For != null)
            {
                _compiledFor = this.For.Compile();
            }

            this.HeaderText = headerText ?? _memberInfo?.GetDisplayName();

            this.IsSortableOverride = isSortable.HasValue ?
                isSortable.Value ?
                    this.IsSortable :
                    false :
                null;

            if ((this.IsSortableOverride ?? this.IsSortable) && sortBy.HasValue && sortBy.Value)
            {
                this.SortBy = true;
                this.SortDescending = sortDescending ?? false;
            }

            this.CellHorizontalAlignment = cellHorizontalAlignment ??
                (IsIndexColumn() ?
                    RATableViewCellHorizontalAlignment.Right :
                    GetCellHorizontalAlignment());

            this.HeaderCellHorizontalAlignment = headerCellHorizontalAlignment ??
                RATableViewCellHorizontalAlignment.Left;

            this.CellClass = cellClass;
        }

        [MemberNotNull(nameof(DisplayValueOptions))]
        internal void EnsureDisplayValueOptions()
        {
            if (this.DisplayValueOptions == null)
            {
                this.DisplayValueOptions = new DisplayValueOptions();
                this.ConfigureDisplayValueOptions?.Invoke(this, this.DisplayValueOptions);
            }
        }

        internal bool CanBeSorted(
            bool? templateIsSortableOverride)
        {
            return ((!templateIsSortableOverride.HasValue || 
                     templateIsSortableOverride.Value) && 
                    this.IsSortable) ||
                   (templateIsSortableOverride.HasValue && 
                    !templateIsSortableOverride.Value && 
                    this.IsSortableOverride.HasValue && 
                    this.IsSortableOverride.Value);
        }

        internal string? GetItemDisplayValue(
            TItem item,
            TimeConverter timeConverter)
        {
            var value = GetForValue(item);
            return GetDisplayValue(value, timeConverter);
        }

        internal string? GetDisplayValue(
            object? value,
            TimeConverter timeConverter)
        {
            var displayValue = value.GetDisplayValue(
                timeConverter,
                propertyName: _memberInfo?.Name,
                options: this.DisplayValueOptions);

            if (!displayValue.HasText())
            {
                displayValue = this.DefaultValue;
            }

            return displayValue;
        }

        internal Type? GetForType()
        {
            if (this.For?.Body is ConditionalExpression conditionalExpression &&
                conditionalExpression.IfTrue is UnaryExpression uranyExpression)
            {
                return uranyExpression.Operand.Type;
            }
            else if (this.For?.Body is UnaryExpression unaryExpression)
            {
                return unaryExpression.Operand.Type;
            }

            return this.For?.Body?
                .GetMemberInfo()?
                .GetType();
        }

        internal object? GetForValue(
            TItem item)
        {
            return _compiledFor?.Invoke(item);
        }

        internal bool IsIndexColumn()
        {
            return this.For == null &&
                   this.HeaderText == INDEX_COLUMN_HEADER;
        }

        private RATableViewCellHorizontalAlignment GetCellHorizontalAlignment()
        {
            if (_memberInfo != null)
            {
                var propertyInfo = _memberInfo as PropertyInfo;
                if (propertyInfo != null &&
                    (propertyInfo.PropertyType.IsBaseOrNullableNumericType() ||
                     propertyInfo.PropertyType.IsBaseOrNullableTemporalType()))
                {
                    return RATableViewCellHorizontalAlignment.Right;
                }
            }
            else
            {

            }

            return RATableViewCellHorizontalAlignment.Left;
        }
    }
}