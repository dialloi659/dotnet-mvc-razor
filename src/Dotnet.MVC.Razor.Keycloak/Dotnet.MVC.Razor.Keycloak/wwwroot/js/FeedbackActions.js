// Business logic for feedback operations
const FeedbackActions = {
    // State management
    archiveState: {
        itemId: null,
        isArchived: null,
        modalId: null
    },

    /**
     * Show archive/unarchive confirmation modal
     */
    showArchiveModal(itemId, isArchived) {
        this.archiveState.itemId = itemId;
        this.archiveState.isArchived = isArchived;
        this.archiveState.modalId = isArchived ? 'confirmArchiveModal' : 'confirmUnarchiveModal';

        TableUtils.showModal(this.archiveState.modalId);
    },

    /**
     * Execute archive/unarchive operation
     */
    executeArchive() {
        if (!this.archiveState.itemId || this.archiveState.isArchived === null) return;

        const { itemId, isArchived, modalId } = this.archiveState;
        const archiveString = isArchived ? 'archived' : 'unarchived';

        TableUtils.makeAjaxRequest('/admin/feedback/Archive', {
            id: itemId,
            isArchived: isArchived
        })
            .done(() => {
                toastr.success(`Feedback ${archiveString}`);
                // Optionally reload table
                // dataTableManager.reloadTable('feedbackTable', false);
            })
            .fail(() => {
                toastr.error(`Failed to ${archiveString} feedback`);
            })
            .always(() => {
                this._resetArchiveState();
                TableUtils.hideModal(modalId);
            });
    },

    /**
     * Update feedback status
     */
    updateStatus(id, newStatus, selectElement) {
        selectElement.prop('disabled', true);

        TableUtils.makeAjaxRequest('/admin/feedback/UpdateStatus', {
            id: id,
            status: newStatus
        })
            .done(() => {
                toastr.success("Status updated");
                dataTableManager.reloadTable('feedbackTable', false);
            })
            .fail(() => {
                toastr.error("Failed to update status");
            })
            .always(() => {
                selectElement.prop('disabled', false);
            });
    },

    /**
     * Initialize event handlers for modals
     */
    initializeModalHandlers() {
        $('#confirmArchiveBtn').on('click', () => {
            this.executeArchive();
        });

        $('#confirmUnarchiveBtn').on('click', () => {
            this.executeArchive();
        });
    },

    /**
     * Reset archive state
     */
    _resetArchiveState() {
        this.archiveState.itemId = null;
        this.archiveState.isArchived = null;
        this.archiveState.modalId = null;
    }
};