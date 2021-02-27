var BTCPayServerStream = BTCPayServerStream || {};

BTCPayServerStream.ShowDialog = function (method, url, titleElementId, contentElementId, requestData) {
    $('#' + titleElementId).html('');
    $('#' + contentElementId).html('<div class="loader loader-wrapped"><div></div><div></div><div></div><div></div></div>');

    $.ajax({
        type: method,
        data: requestData,
        url: url,
        cache: false,
        success: function (result) {
            $('#' + contentElementId).removeClass('modal-loading');

            $('#' + titleElementId)
                .hide()
                .html(result.title)
                .fadeIn('fast');
            $('#' + contentElementId)
                .hide()
                .html(result.content)
                .fadeIn('fast');
        },
        error: function (xhr, status, error) {
            $('#' + contentElementId).html('Error while sending data to server.');
        }
    });
};

BTCPayServerStream.SetFilesEvents = function (formWrapper) {
    $(formWrapper).find('input[type=file]').on('change', function () {
        $(this).next('span.selected-value').html($(this).val().split('\\').pop());
    });

    $(formWrapper).find('span.file-clear').on('click', function () {
        $(this).parent().children('input').val('');
        $(this).parent().children('span.selected-value').html('');
    });
};

BTCPayServerStream.MessageContext = {
    ERROR: 0,
    WARNING: 1,
    INFO: 2,
    SUCCESS: 3
};

BTCPayServerStream.AddFlashMessage = function (message, context, closable) {
    var classes = 'alert';

    if (typeof context === 'undefined') {
        context = 2;
    }

    if (typeof closable !== 'undefined' && closable === true) {
        classes += ' alert-dismissable';
    }

    switch (context) {
        case 0:
            classes += ' alert-danger';
            break;
        case 1:
            classes += ' alert-warning';
            break;
        case 2:
            classes += ' alert-info';
            break;
        case 3:
            classes += ' alert-success';
            break;
    }

    var element = '<div class="' + classes + '">';
    if (typeof closable !== 'undefined' && closable === true) {
        element += '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>';
    }
    element += message + '</div>';

    var container = $('#FlashMessages .container');
    container.hide();
    if (typeof closable !== 'undefined' && closable === true) {
        container.html(element).fadeIn();
    }
    else {
        container.html(element).fadeIn().delay(1000).fadeOut();
    }
};

BTCPayServerStream.SwitchLocalization = function (culture) {
    ShowPageLoader();

    var requestData = {};
    requestData.culture = culture;

    $.ajax({
        type: 'POST',
        data: requestData,
        url: '/Localization/SetLanguage',
        cache: false,
        success: function (result) {
            location.reload();
        },
        error: function (xhr, status, error) {
        }
    });
};

function ShowPageLoader() {
    $('.site-loader').show();
}

function HidePageLoader() {
    $('.site-loader').hide();
}