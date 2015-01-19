/*!
 * Copyright Selective Analytics 2013-2015. This is NOT open-source
 *
 * These are standard functions for Kendo Grid. Mainly confirmation and error message handling. 
 * It assumes JQuery, the GenericJavaScript NotifySupport.js, templates and Kendo are loaded
 */

var grids = (function($,notify) {
    "use strict";

    var $grid = $('#PrimaryKGrid');         //Assumes Kendo grid grid is is given name PrimaryKGrid
    var $messageTemplate = $("#message");   //There is a Kendo template for the grid with id message

    var requestEndAddedFunction = null;

    var updatePrimaryMessage = function(message, isError) {
        if (isError === true) {
            notify.showSingleMessage(message);
        } else {
            notify.showSingleMessage(message, 'success');
        }
    };

    var showMessage = function(editableElements, name, errors) {
        if (name === '' || editableElements === null) {
            //Add general message to top of form
            updatePrimaryMessage(errors[0], true);
        } else {
            //add the validation message to the form
            $grid.find('[data-valmsg-for=' + name + '],[data-val-msg-for=' + name + ']')
                .replaceWith(kendo.template($messageTemplate.html())({ field: name, message: errors[0] }));
        }
    };

    //public items

    return {
        requestStart: function(e) {
            notify.hideNotify();
        },

        requestEnd: function(e) {
            var successMessage;

            if (e.response === undefined || e.type === 'read') {
                //If the function is a read then we don't say anything
                return;
            }

            if (!e.response.Errors) {
                //If no errors then put up a success message
                successMessage = e.response.SuccessMessage || 'The ' + e.type + ' item was successful.';
                updatePrimaryMessage(successMessage, false);
            }

            //if the developer has added a function at the end then call it
            if (requestEndAddedFunction != null)
                requestEndAddedFunction(e, $grid.data('kendoGrid'));
        },

        //This allows an extra function to be called at the end of a non-read requestEnd
        //The fucntion is called with two parametrs: a) the requestEnd arg and b) a refence to the kendo grid
        addExtraActionAtEndOfNonRead: function(func) {
            requestEndAddedFunction = func;
        },

        dataBound: function() {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        },

        errorHandler: function(args) {

            var grid;
            $grid.data('kendoGrid').cancelChanges(); //we have to manually cancel any changes

            if (typeof args.errors === 'undefined') {
                if (args.xhr.status === 403) {
                    updatePrimaryMessage('You are not authorised to carry out that operation.', true);
                } else if (args.xhr.status === 401) {
                    updatePrimaryMessage('You need to be logged in to carry out that operation.', true);
                } else if (args.xhr.status === 500) {
                    updatePrimaryMessage('The operation did not complete because of an error.', true);
                } else {
                    updatePrimaryMessage('There was an unknown problem. I could not complete the action.', true);
                }               
            } else {
                grid = $('#PrimaryKGrid').data('kendoGrid');
                grid.one('dataBinding', function(e) {
                    e.preventDefault();
                    //We have formatted errors to display
                    for (var error in args.errors) {
                        showMessage(grid.editable, error, args.errors[error].errors);
                    }
                });
            }
        },

        sendAntiForgery: function() {
            return { "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val() };
        }
    };

}(window.jQuery, notify));