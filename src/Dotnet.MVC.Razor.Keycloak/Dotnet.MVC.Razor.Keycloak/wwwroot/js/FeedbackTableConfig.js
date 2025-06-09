// Configuration for feedback table
const FeedbackTableConfig = {
    tableId: 'feedbackTable',

    ajax: {
        //url: '/admin/feedback/FeedbackReports',
        url: '/admin/feedback/FeedbackReports/Index',
        type: 'GET',
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        },
        filterData: function (d) {
            return {
                'Filters.CategoryId': $('#categoryFilter').val(),
                'Filters.Status': $('#statusFilter').val(),
                'Filters.IsArchived': $('#archivedFilter').val(),
                'Filters.Draw': d.draw
            };
        }
    },

    columns: [
        { data: 'categoryName' },
        { data: 'email' },
        {
            data: 'pageUrl',
            render: url => `<a href="${url}" target="_blank">View</a>`
        },
        { data: 'message' },
        {
            data: 'creationDate',
            render: d => new Date(d).toLocaleString()
        },
        {
            data: 'status',
            render: function (status, type, row) {
                const options = ['New', 'InProgress', 'Processed', 'NotInteresting']
                    .map(s => `<option value="${s}" ${s === status ? 'selected' : ''}>${s}</option>`)
                    .join('');

                return `<select class="form-select form-select-sm status-select" data-id="${row.id}">
                    ${options}
                </select>`;
            }
        },
        {
            data: null,
            orderable: false,
            searchable: false,
            render: function (row) {
                const isArchived = row.isArchived === true;
                const groupName = `archiveStatus_${row.id}`;

                return `
                    <div class="btn-group btn-group-sm" role="group" aria-label="Archive status">
                        <input type="radio" class="btn-check archive-radio-active"
                                name="${groupName}" id="active_${row.id}"
                                data-id="${row.id}" value="false" autocomplete="off"
                                ${!isArchived ? 'checked' : ''}>
                        <label class="btn btn-outline-success" for="active_${row.id}">Active</label>

                        <input type="radio" class="btn-check archive-radio-archived"
                                name="${groupName}" id="archived_${row.id}"
                                data-id="${row.id}" value="true" autocomplete="off"
                                ${isArchived ? 'checked' : ''}>
                        <label class="btn btn-outline-secondary" for="archived_${row.id}">Archived</label>
                    </div>
                `;
            }
        },
        {
            data: 'id',
            orderable: false,
            searchable: false,
            render: id => `
                <button class="btn btn-sm btn-outline-secondary toggle-status-btn" data-id="${id}">
                    <i class="fa fa-ellipsis"></i>
                </button>`
        }
    ],

    filters: [
        { selector: '#categoryFilter' },
        { selector: '#statusFilter' },
        { selector: '#archivedFilter' }
    ],

    rowCallback: function (row, data) {
        if (data.isArchived) {
            $(row).addClass('table-secondary text-muted');
        } else {
            $(row).removeClass('table-secondary text-muted');
        }
    },

    eventHandlers: {
        'change': {
            selector: '.archive-radio-archived',
            callback: function () {
                const isArchiveValue = $(this).val() === 'true';
                if (!isArchiveValue) return;

                FeedbackActions.showArchiveModal($(this).data('id'), true);
            }
        },
        'change .archive-radio-active': {
            selector: '.archive-radio-active',
            callback: function () {
                const isActiveValue = $(this).val() === 'false';
                if (!isActiveValue) return;

                FeedbackActions.showArchiveModal($(this).data('id'), false);
            }
        },
        'change .status-select': {
            selector: '.status-select',
            callback: TableUtils.debounce(function () {
                const select = $(this);
                const id = select.data('id');
                const newStatus = select.val();

                FeedbackActions.updateStatus(id, newStatus, select);
            }, 500)
        }
    }
};