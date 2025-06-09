
$(function () {
    const $sidebar = $('#feedback-sidebar');
    const $button = $('#open-feedback-btn');
    const $close = $('#close-feedback-btn');
    const $form = $('#feedback-form');
    const $spinner = $('#feedback-spinner');
    const $submitText = $('#feedback-submit-text');

    const $successMessage = $('#feedback-success-message');
    const $closeSuccessBtn = $('#close-feedback-success');

    const clearValidation = () => {
        $form.find('.is-invalid, .is-valid').removeClass('is-invalid is-valid');
        $form.find('.invalid-feedback, .valid-feedback').addClass('d-none').text('');
    };

    const showFieldError = (fieldName, messages) => {
        const $field = $form.find(`[name="${fieldName}"]`);
        const $errorDiv = $field.closest('.mb-3').find('.invalid-feedback');
        $field.addClass('is-invalid');
        $errorDiv.text(messages.join(', ')).removeClass('d-none');
    };

    const markFieldValid = (fieldName) => {
        const $field = $form.find(`[name="${fieldName}"]`);
        const $validDiv = $field.closest('.mb-3').find('.valid-feedback');
        if (!$field.hasClass('is-invalid')) {
            $field.addClass('is-valid');
            $validDiv.removeClass('d-none');
        }
    };

    $button.on('click', function () {
        resetSidebar(); // ← Makes sure previous success/errors don't persist
        $('#CurrentPage').val(window.location.pathname);
        $sidebar.show().addClass('animate__animated animate__slideInRight');
    });

    $close.on('click', function () {
        $sidebar.removeClass('animate__slideInRight').hide();
    });

    $closeSuccessBtn.on('click', function () {
        closeSidebar();
    });

    $form.on('submit', function (e) {
        e.preventDefault();
        clearValidation();
        $spinner.removeClass('d-none');
        $submitText.addClass('d-none');

        $.ajax({
            url: '/Feedback/Create',
            method: 'POST',
            data: $form.serialize(),
            success: function () {
                toastr.success("Feedback sent successfully.");
                $form.trigger('reset').hide();
                $successMessage.removeClass('d-none');
            },
            error: function (xhr) {
                if (xhr.status === 400 && xhr.responseJSON?.errors) {
                    const errors = xhr.responseJSON.errors;
                    const allFields = ['Email', 'Category', 'Message'];

                    allFields.forEach(field => {
                        if (errors[field]) {
                            showFieldError(field, errors[field]);
                        } else {
                            markFieldValid(field);
                        }
                    });
                } else {
                    toastr.error("An error occurred. Please try again.");
                }
            },
            complete: function () {
                $spinner.addClass('d-none');
                $submitText.removeClass('d-none');
            }
        });
    });

    function resetSidebar() {
        $form.trigger('reset').show();                   // Clear form inputs and show form
        $successMessage.addClass('d-none');              // Hide success message    // Clear and hide error list
        clearValidationState();                          // Clear Bootstrap validation classes/messages
    }

    function closeSidebar() {
        $sidebar.removeClass('animate__slideInRight').hide();
        resetSidebar(); // Ensures all sidebar components are reset
    }

    function clearValidationState() {
        $('#Category, #Message').removeClass('is-invalid is-valid');
        $('.invalid-feedback, .valid-feedback').addClass('d-none').text('');
    }

});
