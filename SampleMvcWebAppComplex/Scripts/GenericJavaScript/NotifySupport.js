/*!
 * Copyright Selective Analytics 2014. This is NOT open-source
 *
 * This provides notification support using the Kendo JavaScript kendoNotification module
 * 
 * It assumes JQuery is loaded before this is defined. Kendo must be loaded before it is called.
 */

var notify = (function ($, kendo) {
    "use strict";

    var $putNotityHere = $('#put-notify-here');
    var $notifySpan = $('#notify-span');

    var stayUpNotifier = $notifySpan.kendoNotification({
        appendTo: $putNotityHere,
        autoHideAfter: 0,           //turn off auto hide
        button: true
    }).data("kendoNotification");
    var autoHideNotifier = $notifySpan.kendoNotification({
        appendTo: $putNotityHere,
    }).data("kendoNotification");


    function localShowSingleMessage(messageString, messageType) {
        var notifytype = messageType || 'error';
        if (notifytype === 'error') {
            stayUpNotifier.show(messageString, notifytype);
        } else {
            autoHideNotifier.show(messageString, notifytype);
        }
    };

    //this is designed to show the error part of an ISuccessErrors object
    function showErrorsPart(errorsPart) {
        for (var error in errorsPart) {
            var prefix = error === '' ? '' : error + ': ';          //it has named parameter, so include it
            localShowSingleMessage(prefix + errorsPart[error].errors);
        }
    }


    //public items

    return {
        
        //This takes a string to display using the Kendo Notification widget
        //The second string holds the possible message types: info, success, warning or error
        //These control the format of the display and how long it is displayed. Defaults to error
        showSingleMessage: function (messageString, messageType) {
            localShowSingleMessage(messageString, messageType);
        },
        //This expects an object in the format of a ISuccessOrError JSON format
        showISuccessOrErrors: function (status) {
            this.hideNotify();
            if (status.SuccessMessage) {
                this.showSingleMessage(status.SuccessMessage, 'success');
            } else if (status.Errors) {
                showErrorsPart(status.Errors);
            } else {
                this.showSingleMessage("Expected a SuccessMessage, but was not present");
            }
        },

        showErrorResponse: function (xhr) {
            this.hideNotify();      //clear preceeding errors
            var json = xhr.responseJSON;
            if (!json || !json.Errors) {
                if (xhr.status === 403) {
                    this.showSingleMessage('You are not authorised to carry out that operation.');
                } else if (xhr.status === 401) {
                    this.showSingleMessage('You need to be logged in to carry out that operation.');
                } else if (xhr.status === 500) {
                    this.showSingleMessage('The operation did not complete because of an error.');
                } else {
                    this.showSingleMessage('There was an unknown problem. I could not complete the action.');
                }
            } else {
                showErrorsPart(json.Errors);
            }
        },

        //This will hide all notify messages
        hideNotify: function() {
            stayUpNotifier.hide();
            autoHideNotifier.hide();
        }

    };

}(window.jQuery, window.kendo));