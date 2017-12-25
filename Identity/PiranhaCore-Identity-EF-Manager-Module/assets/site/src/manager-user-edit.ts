
ko.bindingHandlers.CheckBox = {
    init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        var value = valueAccessor();
        var claimValue = ko.unwrap(value);
        var userId = bindingContext.$root.UserId;
        $(element).on('change',
            () => {
                var $label = $(element).closest("label");
                var $loading =
                    $('<span>&nbsp;<i class="fa fa-circle-o-notch fa-spin" style="color: #439700;"></i></span>');
                $loading.appendTo($label);

                var isChecked = $(element).prop("checked");
                var postData = {
                    Id: userId,
                    ClaimValue: claimValue,
                    Add: isChecked
                }

                $.post(`/manager/user/claims/update`, postData).done((data: any) => {
                    $loading.remove();
                });
            });
    }
}
// Edit View Model
class UserEditViewModel {
    UserClaims: KnockoutObservableArray<any> = ko.observableArray([]);
    AllClaims: KnockoutObservableArray<any> = ko.observableArray([]);
    UserId: any;

    constructor(userClaims: any, allClaims: any, userId?:any) {
        if(userId) {
            this.UserId = userId;
        }
        if (userClaims) {
            ko.mapping.fromJS(userClaims, {}, this.UserClaims)
        }
        if (allClaims) {
            ko.mapping.fromJS(allClaims, {}, this.AllClaims);
        }
    }
}