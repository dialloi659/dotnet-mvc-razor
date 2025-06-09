// DataTableManager.js - Central utility for managing DataTables
class DataTableManager {
    constructor() {
        this.tables = new Map();
        this.defaultConfig = {
            processing: true,
            serverSide: true,
            responsive: true,
            lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]]
        };
    }

    /**
     * Initialize a DataTable with custom configuration
     * @param {string} selector - Table selector (e.g., '#feedbackTable')
     * @param {Object} config - Configuration object
     */
    init(selector, config = {}) {
        const tableId = selector.replace('#', '');

        // Merge with default config
        const tableConfig = {
            ...this.defaultConfig,
            ...config,
            ajax: this._buildAjaxConfig(config.ajax, tableId),
            columns: config.columns || []
        };

        // Add row callback if provided
        if (config.rowCallback) {
            tableConfig.rowCallback = config.rowCallback;
        }

        const table = $(selector).DataTable(tableConfig);
        this.tables.set(tableId, {
            table: table,
            config: config,
            nextCursor: null
        });

        // Setup filters if provided
        if (config.filters) {
            this._setupFilters(tableId, config.filters);
        }

        // Setup custom event handlers
        if (config.eventHandlers) {
            this._setupEventHandlers(selector, config.eventHandlers);
        }

        return table;
    }

    /**
     * Build AJAX configuration with cursor support and filters
     */
    _buildAjaxConfig(ajaxConfig, tableId) {
        //cursor paging
        //const self = this;

        return {
            url: ajaxConfig.url,
            type: ajaxConfig.type || 'GET',
            headers: ajaxConfig.headers || {},
            data: function (tableRequest) {
                // Cursor based paging
                //const tableData = self.tables.get(tableId);
                const pageIndex = (tableRequest.length > 0) ? tableRequest.start / tableRequest.length : 0;
                const requestData = {
                    //Cursor: tableData?.nextCursor,
                    //PageIndex: tableRequest.start,
                    PageIndex: pageIndex,
                    PageSize: tableRequest.length,
                    GlobalSearch: tableRequest.search.value,
                    OrderBy: tableRequest.columns[tableRequest.order[0].column].data,
                    OrderDescending: tableRequest.order[0].dir === 'desc'
                };

                // Add filter data if configured
                if (ajaxConfig.filterData) {
                    Object.assign(requestData, ajaxConfig.filterData(tableRequest));
                }

                return requestData;
            },
            dataSrc: function (json) {
                //Set cursor value
                //const tableData = self.tables.get(tableId);
                //tableData?.nextCursor = json.nextCursor;

                return json.data;
            }
        };
    }

    /**
     * Setup filter controls
     */
    _setupFilters(tableId, filters) {
        const table = this.tables.get(tableId).table;

        filters.forEach(filter => {
            $(filter.selector).on('change', () => {
                this.resetCursor(tableId);
                table.ajax.reload();
            });
        });
    }

    /**
     * Setup custom event handlers for table rows
     */
    _setupEventHandlers(selector, handlers) {
        Object.entries(handlers).forEach(([event, handler]) => {
            $(selector).on(event, handler.selector, handler.callback);
        });
    }

    /**
     * Reset cursor for a specific table
     */
    resetCursor(tableId) {
        if (this.tables.has(tableId)) {
            this.tables.get(tableId).nextCursor = null;
        }
    }

    /**
     * Get table instance
     */
    getTable(tableId) {
        return this.tables.get(tableId)?.table;
    }

    /**
     * Reload table data
     */
    reloadTable(tableId, resetPaging = false) {
        const table = this.getTable(tableId);
        if (table) {
            table.ajax.reload(null, resetPaging);
        }
    }
}

// Utility functions
const TableUtils = {
    debounce(fn, delay) {
        let timeout;
        return function () {
            const context = this;
            const args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => fn.apply(context, args), delay);
        };
    },

    showModal(modalId) {
        const modal = new bootstrap.Modal(document.getElementById(modalId));
        modal.show();
    },

    hideModal(modalId) {
        const modalInstance = bootstrap.Modal.getInstance(document.getElementById(modalId));
        if (modalInstance) {
            modalInstance.hide();
        }
    },

    makeAjaxRequest(url, data, options = {}) {
        const defaultOptions = {
            type: 'POST',
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        };

        return $.ajax({
            url: url,
            data: data,
            ...defaultOptions,
            ...options
        });
    }
};

// Export for use
window.DataTableManager = DataTableManager;
window.TableUtils = TableUtils;