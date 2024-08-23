namespace Orbital7.Extensions.AspNetCore.RapidApp;

public class RATableTemplate
{
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
                    sortBy: sortByNames,
                    isSortable: isNamesSortable),
                new(
                    x => x.Value,
                    isSortable: isValuesSortable,
                    cellHorizontalAlignment: RATableViewCellHorizontalAlignment.Right,
                    headerCellHorizontalAlignment: RATableViewCellHorizontalAlignment.Right),
            ]);
    }
}

public class RATableTemplate<TEntity> :
    RATableTemplate
{
    public List<Column<TEntity>> Columns { get; private set; }

    public Action<RATableViewSegment<TEntity>, TEntity> OnRowSelected { get; init; }

    public bool IsSortable { get; init; }

    public bool HasFooter => this.Columns.Count > 0 &&
        (this.Columns.Any(x => x.GetFooterCellValue != null) ||
         this.Columns.Any(x => x.GetFooterCellContent != null));

    public RATableTemplate(
        List<Column<TEntity>> columns,
        Action<RATableViewSegment<TEntity>, TEntity> onRowSelected = null,
        bool isSortable = true)
    {
        this.Columns = columns;
        this.OnRowSelected = onRowSelected;
        this.IsSortable = isSortable;
    }

    public ICollection<TEntity> GetSortedItems(
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
                    sortColumn = this.Columns
                        .Where(x =>
                            x.IsSortable &&
                            x.For?.Body != null)
                        .FirstOrDefault();
                }

                // Sort if we have a column to sort by.
                if (sortColumn != null)
                {
                    var sortedItems = sortColumn.SortDescending ?
                        segment.Items.AsQueryable().OrderByDescending(sortColumn.For).ToList() :
                        segment.Items.AsQueryable().OrderBy(sortColumn.For).ToList();

                    return sortedItems;
                }
            }
            
            return segment.Items;
        }

        return null;
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

    private Column<TEntity> GetCurrentSortColumn()
    {
        return this.Columns
            .Where(x => 
                x.SortBy && 
                x.IsSortable && 
                x.For?.Body != null)
            .FirstOrDefault();
    }

    public class Column<TItem>
    {
        private Func<TItem, object> _compiledFor;

        public string HeaderText { get; set; }

        public bool IsSortable { get; set; }

        public bool SortDescending { get; set; } = false;

        public bool SortBy { get; set; } = false;

        public string CellClass { get; set; }

        public int? FixedWidth { get; set; }

        public Expression<Func<TItem, object>> For { get; set; }

        public Action<Column<TItem>, RATableViewSegment<TItem>, TItem> OnCellUrlClicked { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, string> GetCellUrl { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, string> GetCellClass { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, string> GetCellStyle { get; set; }

        public Func<Column<TItem>, RATableViewSegment<TItem>, TItem, object> GetCellValue { get; set; }

        public RenderFragment<TItem> GetCellContent { get; set; }

        public RenderFragment<List<RATableViewSegment<TEntity>>> GetHeaderCellContent { get; set; }

        public RenderFragment<RATableViewFooterData<TItem>> GetFooterCellContent { get; set; }

        public Func<RATableViewFooterData<TItem>, object> GetFooterCellValue { get; set; }

        public Func<RATableViewFooterData<TItem>, string> GetFooterCellClass { get; set; }

        public DisplayValueOptions DisplayValueOptions { get; set; }

        public Action<Column<TItem>, DisplayValueOptions> ConfigureDisplayValueOptions { get; set; }

        public string DefaultValue { get; set; } = "-";

        public RATableViewCellHorizontalAlignment CellHorizontalAlignment { get; set; }

        public RATableViewCellHorizontalAlignment HeaderCellHorizontalAlignment { get; set; }

        public Column(
            Expression<Func<TItem, object>> forValue,
            string headerText = null,
            bool? sortBy = null,
            bool? isSortable = null,
            bool? sortDescending = null,
            RATableViewCellHorizontalAlignment? cellHorizontalAlignment = null,
            RATableViewCellHorizontalAlignment? headerCellHorizontalAlignment = null,
            string cellClass = null)
        {
            var memberInfo = forValue?.Body.GetPropertyInformation();
            this.IsSortable = isSortable ?? memberInfo != null;

            this.For = forValue;
            if (this.For != null)
            {
                _compiledFor = this.For.Compile();
            }

            this.HeaderText = headerText ?? memberInfo?.GetDisplayName();

            if (this.IsSortable && sortBy.HasValue && sortBy.Value)
            {
                this.SortBy = true;
                this.SortDescending = sortDescending ?? false;
            }

            if (this.DisplayValueOptions == null)
            {
                this.DisplayValueOptions = new();
                this.ConfigureDisplayValueOptions?.Invoke(this, this.DisplayValueOptions);
            }

            this.CellHorizontalAlignment = cellHorizontalAlignment ?? 
                GetCellHorizontalAlignment(memberInfo);

            this.HeaderCellHorizontalAlignment = headerCellHorizontalAlignment ?? 
                RATableViewCellHorizontalAlignment.Left;

            this.CellClass = cellClass;
        }

        internal string GetItemDisplayValue(
            TItem item,
            TimeConverter timeConverter)
        {
            var value = GetForValue(item);
            return GetDisplayValue(value, timeConverter);
        }

        internal string GetDisplayValue(
            object value,
            TimeConverter timeConverter)
        {
            var displayValue = value.GetDisplayValue(
                timeConverter,
                propertyName: this.For?.Name,
                options: this.DisplayValueOptions);

            if (!displayValue.HasText())
            {
                displayValue = this.DefaultValue;
            }

            return displayValue;
        }

        internal Type GetForType()
        {
            if (this.For?.Body is ConditionalExpression)
            {
                var conditionalExpression = this.For.Body as ConditionalExpression;
                var ifTrueExpression = conditionalExpression.IfTrue as UnaryExpression;
                if (ifTrueExpression != null)
                {
                    return ifTrueExpression.Operand.Type;
                }
            }
            
            return this.For?.Body?
                .GetPropertyInformation()?
                .GetType();
        }

        internal object GetForValue(
            TItem item)
        {
            return _compiledFor?.Invoke(item);
        }

        private RATableViewCellHorizontalAlignment GetCellHorizontalAlignment(
            MemberInfo memberInfo)
        {
            if (memberInfo != null)
            {
                var propertyInfo = memberInfo as PropertyInfo;
                if (propertyInfo != null && propertyInfo.PropertyType.IsBaseOrNullableNumericType())
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