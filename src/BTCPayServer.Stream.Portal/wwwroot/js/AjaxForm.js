(function ($) {
    $.fn.AjaxForm = function (params) {
        var form = this;

        var action = this.attr('action');
        var method = this.attr('method');

        form.submit(function (e) {
            e.preventDefault();
            ShowPageLoader(true);
            form.ajaxSubmit({
                url: action,
                type: method,
                success: function (data) {
                    ClearErrorMessages(form);
                    if (data.success === true) {
                        if (typeof data.conditioned !== 'undefined' && data.conditioned === true) {
                            if (typeof params !== 'undefined' && typeof params.onCondition === 'function') {
                                params.onCondition(data.conditions);
                            }
                            else {
                                for (var i = 0; i < data.conditions.length; i++) {
                                    if ($('#' + data.conditions[i].field + 'Group').length) {
                                        $('#' + data.conditions[i].field + 'Group').remove();
                                    }
                                    $('#' + data.conditions[i].formGroup).after('<div class="form-group" id="' + data.conditions[i].field + 'Group"><div id="' + data.conditions[i].field + 'Btn" data-inputField="' + data.conditions[i].inputField + '" class="form-control btn btn-default">' + data.conditions[i].buttonText + '</div></div>');

                                    $('#' + data.conditions[i].field + 'Btn').on('click', function () {
                                        var id = $(this).attr('id').replace('Btn', '');
                                        $('#' + id).val('True');
                                        $('span[data-condition-group=' + id + ']').html('');
                                        $(this).parent().remove();
                                    });

                                    $('span[data-valmsg-for=' + data.conditions[i].inputField + ']').html(data.conditions[i].message);
                                }
                            }

                            HidePageLoader();
                        }
                        else if (typeof params !== 'undefined' && typeof params.onSuccess === 'function') {
                            params.onSuccess(data);

                            if (typeof params.preventHigingLoader === 'undefined' || params.preventHigingLoader === false) {
                                HidePageLoader();
                            }
                            
                            if (typeof data.data !== 'undefined' && data.data !== null && typeof data.data.message !== 'undefined') {
                                BTCPayServerStream.AddFlashMessage(data.data.message, BTCPayServerStream.MessageContext.SUCCESS);
                            }
                        }
                        else {
                            window.location = data.url;
                        }
                    }
                    else {
                        HidePageLoader();
                        for (var j = 0; j < data.errors.length; j++) {
                            $('input[name=' + data.errors[j].field + ']').addClass('is-invalid');
                            $('textarea[name=' + data.errors[j].field + ']').addClass('is-invalid');
                            $('input[name=' + data.errors[j].field + ']').parent('.people-picker-container').addClass('input-validation-error');
                            var parent = $('input[name=' + data.errors[j].field + ']').parent();
                            if (parent.hasClass('file-area-field')) {
                                parent.addClass('input-validation-error');
                            }
                            $('select[name=' + data.errors[j].field + ']').addClass('is-invalid');
                            $('span[data-valmsg-for=' + data.errors[j].field + ']').html(data.errors[j].message);
                        }
                        if (typeof params !== 'undefined' && typeof params.onInvalid === 'function') {
                            params.onInvalid();
                        }
                    }
                },
                error: function (data) {
                    HidePageLoader();
                    $('.modal').modal('hide');
                    BTCPayServerStream.AddFlashMessage('Something went wrong.', BTCPayServerStream.MessageContext.ERROR, true);
                }
            });
        });

        return this;
    };

    function ClearErrorMessages(form) {
        form.find('.is-invalid').removeClass('is-invalid');
        form.find('.input-validation-error').removeClass('input-validation-error');
        form.find('span.text-danger').html('');
    }
})(jQuery);