function getRootUrl() {
    return '';// /MBVABBank
}
var systemSearchApp = angular.module("systemSearchApp", ['ui.bootstrap']);
systemSearchApp.filter('PageStart', function ()
{
    return function (input, start)
    {
        if (input)
        {
            start = +start;
            return input.slice(start);
        }
        return [];
    };
});

systemSearchApp.controller("softOtpActivationSmsController", function ($scope, $http, $timeout) {
    getSoftActivationSmsList();
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    function getSoftActivationSmsList() {
        var model = {
            MobileNo: $('#mobile-no').val(),
            Status: $('#status').val(),
            Frd: $('#fromDate').val(),
            Td: $('#toDate').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/SoftOtpActivationSms/GetListSoftOtpActivationSmsHistory",
            params: {
                mobileNo: JSON.stringify(model.MobileNo),
                status: JSON.stringify(model.Status),
                frd: JSON.stringify(model.Frd),
                td: JSON.stringify(model.Td),
                pageIndex: JSON.stringify($scope.currentPage),
                pageSize: JSON.stringify($scope.PerPageItems)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
        
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
    $scope.viewDetail = function (item)
    {
        $http({
            method: "post",
            url: getRootUrl() + "/SoftOtpActivationSms/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg)
        {

            if (redirectToLogin(msg))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                var model = msg.data;
                document.getElementById('info-mobileno').innerHTML = model.MobileNo;
                document.getElementById('info-status').innerHTML = model.Status;
                document.getElementById('info-sentday').innerHTML = model.SentDate;
                document.getElementById('info-receiveddate').innerHTML = model.ReceivedDate;
                document.getElementById('info-content').innerHTML = model.Content;
                               //document.getElementById('info-branchbene').innerHTML = model.BeneBranch;
                $('#pharse-list tr').has('td').remove();
                //if (model.PharseList.length > 0) {
                //    for (var i = 0; i < model.PharseList.length; i++) {
                //        var tr = document.getElementById('pharse-list').insertRow(i + 1);
                //        var tdTranxDetailId = tr.insertCell(0);
                //        tdTranxDetailId.innerHTML = model.PharseList[i].TranxDetailId;
                //        var tdPharse = tr.insertCell(1);
                //        tdPharse.innerHTML = model.PharseList[i].Pharse;
                //        var tdResCode = tr.insertCell(2);
                //        tdResCode.innerHTML = model.PharseList[i].ResCode;
                //        var tdCreatedDate = tr.insertCell(3);
                //        tdCreatedDate.innerHTML = model.PharseList[i].CreatedDate;
                //        var tdTranxNote = tr.insertCell(4);
                //        tdTranxNote.innerHTML = model.PharseList[i].TranxNote;
                //    }
                //}
                $('#viewDetail').modal("show");
            }
        });
    };
    $scope.getSoftActivationSmsList = function () {
        getSoftActivationSmsList();
    };
    $scope.pageChanged = function () {
        getSoftActivationSmsList();
    };
   

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status")) {
            getCurrencyList();
        }
    });


});

systemSearchApp.controller("SoftOTPTransactionManagermentController", function ($scope, $http, $timeout) {
    getSoftTranxList();
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    //$scope.billservicetypeSearch = function () {
    //    getBillServiceTypeList();
    //};
    function getSoftTranxList() {
        var model = {
            TranxId: $('#tranx-id').val(),
            UserName: $('#user-name').val(),
            Channel: $('#channel').val(),
            Status: $('#status').val(),
            Frd: $('#fromDate').val(),
            Td: $('#toDate').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/SoftOTPTransactionManagerment/GetListSoftOtpTransaction",
            params: {
                tranxId: JSON.stringify(model.TranxId),
                userName: JSON.stringify(model.UserName),
                status: JSON.stringify(model.Status),
                channel: JSON.stringify(model.Channel),
                frd: JSON.stringify(model.Frd),
                td: JSON.stringify(model.Td),
                pageIndex: JSON.stringify($scope.currentPage),
                pageSize: JSON.stringify($scope.PerPageItems)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
    $scope.getSoftTranxList = function () {
        getSoftTranxList();
    };
    $scope.pageChanged = function () {
        getSoftTranxList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/SoftOtpRegistration/ChangeStatusSoftOTP",
                params: {
                    userId: JSON.stringify(item.UserId),
                    status: JSON.stringify(item.Status)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getSoftRegisList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status")) {
            getCurrencyList();
        }
    });


});

systemSearchApp.controller("softOtpCancelRegisController", function ($scope, $http, $timeout) {
    getSoftCancelRegisList();
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    //$scope.billservicetypeSearch = function () {
    //    getBillServiceTypeList();
    //};
    function getSoftCancelRegisList() {
        var model = {
            Cif: $('#cif').val(),
            UserName: $('#user-name').val(),
            Channel: $('#channel').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/SoftOtpCancelRegistration/GetListCancelSoftOtpRegistration",
            params: {
                cif: JSON.stringify(model.Cif),
                userName: JSON.stringify(model.UserName),
                channel: JSON.stringify(model.channel),
                pageIndex: JSON.stringify($scope.currentPage),
                pageSize: JSON.stringify($scope.PerPageItems)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
    $scope.getSoftCancelRegisList = function () {
        getSoftCancelRegisList();
    };
    $scope.pageChanged = function () {
        getSoftCancelRegisList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/SoftOtpRegistration/ChangeStatusSoftOTP",
                params: {
                    userId: JSON.stringify(item.UserId),
                    status: JSON.stringify(item.Status)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getSoftRegisList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status")) {
            getCurrencyList();
        }
    });

    
});

systemSearchApp.controller("softOtpRegisController", function ($scope, $http, $timeout) {
    getSoftRegisList();
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    //$scope.billservicetypeSearch = function () {
    //    getBillServiceTypeList();
    //};
    function getSoftRegisList() {
        var model = {
            Cif: $('#cif').val(),
            UserName: $('#user-name').val(),
            Channel: $('#channel').val(),
            Status: $('#status').val(),
            Frd: $('#fromDate').val(),
            Td: $('#toDate').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/SoftOtpRegistration/GetListSoftOtpRegistration",
            params: {
                cif: JSON.stringify(model.Cif),
                userName: JSON.stringify(model.UserName),
                status: JSON.stringify(model.Status),
                channel: JSON.stringify(model.channel),
                frd: JSON.stringify(model.Frd),
                td: JSON.stringify(model.Td),
                pageIndex: JSON.stringify($scope.currentPage),
                pageSize: JSON.stringify($scope.PerPageItems)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
    $scope.getSoftRegisList = function () {
        getSoftRegisList();
    };
    $scope.pageChanged = function () {
        getSoftRegisList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/SoftOtpRegistration/ChangeStatusSoftOTP",
                params: {
                    userId: JSON.stringify(item.UserId),
                    status: JSON.stringify(item.Status)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getSoftRegisList();
                }
            });
        } else {
            return false;
        }
    };
    $scope.changeStatusAuto = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/SoftOtpRegistration/ChangeStatusSoftOTPAuto",
                params: {
                    userId: JSON.stringify(item.UserId),
                    status: JSON.stringify(item.Status),
                    userName: JSON.stringify(item.UserName)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getSoftRegisList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status")) {
            getCurrencyList();
        }
    });

    
});
systemSearchApp.controller("userController", function ($scope, $http, $timeout)
{
    getUserSearch();

    $scope.deleteUser = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của người dùng này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Admin/DeleteUser",
                params: {
                    id: JSON.stringify(item.UserId)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getUserSearch();
                }
            });
        } else {
            return false;
        }
    };

    $scope.changeStatus = function (item)
    {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của người dùng này?";
        var r = confirm(confirmMesg);
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Admin/ChangeStatusUser",
                params: {
                    id: JSON.stringify(item.UserId),
                    username: JSON.stringify(item.UserName),
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index"
                } else
                {
                    //document.getElementById("lblMessage").innerText = msg.data.toString();
                    //document.getElementById("lblMessage").style.color = "blue";
                    alert(msg.data.toString());
                    getUserSearch();
                }
            });
        } else
        {
            return false;
        }
    };
    $scope.resetPassword = function (item)
    {
        var r = confirm("Bạn muốn làm mới mật khẩu của người dùng này?");
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Admin/ResetPassword",
                params: {
                    id: JSON.stringify(item.UserId),
                    username: JSON.stringify(item.UserName),
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index"
                } else
                {
                    //document.getElementById("lblMessage").innerText = msg.data.toString();
                    //document.getElementById("lblMessage").style.color = "blue";
                    alert(msg.data.toString());
                    getUserSearch();
                }
            });
        } else
        {
            return false;
        }
    };
    $scope.assignRole = function (item)
    {
        var roleIds = getRoleList(item.UserId);       
        var response = $http({
            method: "post",
            url: getRootUrl() + "/Admin/AssignRole",
            params: {
                id: JSON.stringify(item.UserId),
                roleIds: JSON.stringify(roleIds)
            }
        });
        response.then(function (msg)
        {
            if (redirectToLogin(msg))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                alert(msg.data.toString());
                window.location.reload();
                //getUserSearch();
                ////var popupid = "#assign_role_popup_"+item.UserId;
                ////$(popupid).dialog("close");              
                //return true;
            }
        });
    };

    $scope.isChecked = function (item, roleId)
    {
        if (item.RoleList.indexOf(roleId) > -1)
        {
            return true;
        }
        return false;
    };

    $scope.userSearch = function ()
    {
        getUserSearch();
    };

    function getUserSearch()
    {
        var model = {
            UserName: $("#id-user-name").val(),
            Status: $("#status").val(),
            BranchCode: $("#branch").val(),
            PosCode: $("#pos").val(),
            Role: $("#role").val(),
            FromDate: $("#fromDate").val(),
            ToDate: $("#toDate").val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Admin/GetListUser",
            params: {
                un: JSON.stringify(model.UserName),
                status: JSON.stringify(model.Status),
                branch: JSON.stringify(model.BranchCode),
                bankpos: JSON.stringify(model.PosCode),
                role: JSON.stringify(model.Role),
                fromdate: JSON.stringify(model.FromDate),
                todate: JSON.stringify(model.ToDate)
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function ()
        {
            $timeout(function ()
            {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };

    }

    function getRoleList(userId)
    {
        var roleList = "";
        var idCheckBox = "#checkAllBoxesRole_" + userId + " input:checked";
        $(idCheckBox).each(function ()
        {
            roleList = roleList + $(this).val() + ",";
        });
        return roleList;

    }
});
systemSearchApp.controller("roleController", function ($scope, $http, $timeout)
{
    getRoleList();

    $scope.roleSearch = function ()
    {
        getRoleList();
    };

    $scope.changeStatus = function (item)
    {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Role/ChangeStatusRole",
                params: {
                    id: JSON.stringify(item.RoleId)
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else
                {
                    //document.getElementById("lblMessage").innerText = msg.data.toString();
                    //document.getElementById("lblMessage").style.color = "blue";
                    alert(msg.data.toString());
                    getRoleList();
                }
            });
        } else
        {
            return false;
        }
    };

    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "id-role-name" || e.target.id === "status-code"))
        {
            getRoleList();
        }
    });
    function getRoleList()
    {
        var model = {
            RoleName: $('#id-role-name').val(),
            RoleStatus: $('#status-code').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Role/GetListRole",
            params: {
                roleName: JSON.stringify(model.RoleName),
                roleStatus: JSON.stringify(model.RoleStatus),
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function ()
        {
            $timeout(function ()
            {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("cityController", function ($scope, $http, $timeout) 
{
    getCityList();

    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "city-name" || e.target.id === "status" || e.target.id === "city-code"))
        {
            getCityList();
        }
    });
    $scope.citySearch = function ()
    {
        getCityList();
    };

    $scope.deleteCity = function (item) {
        var confirmMesg = "Bạn muốn xóa bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/City/DeleteCity",
                params: {
                    id: JSON.stringify(item.CityCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getCityList();
                }
            });
        } else {
            return false;
        }
    };

    $scope.changeStatus = function (item)
    {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/City/ChangeStatusCity",
                params: {
                    cityCode: JSON.stringify(item.CityCode)
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else
                {
                    alert(msg.data.toString());
                    getCityList();
                }
            });
        } else
        {
            return false;
        }
    };

    function getCityList()
    {
        var model = {
            CityName: $('#city-name').val(),
            Status: $('#status').val(),
            CityCode: $('#city-code').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/City/GetListCity",
            params: {
                cityName: JSON.stringify(model.CityName),
                status: JSON.stringify(model.Status),
                cityCode: JSON.stringify(model.CityCode)
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function ()
        {
            $timeout(function ()
            {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("districtController", function ($scope, $http, $timeout) {
    getDistrictList();
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "district-name" || e.target.id === "status" || e.target.id === "district-code" || e.target.id === "city-code")) {
            getDistrictList();
        }
    });
    $scope.districtSearch = function () {
        getDistrictList();
    };

    $scope.deleteDistrict = function (item) {
        var confirmMesg = "Bạn muốn xóa bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/District/DeleteDistrict",
                params: {
                    id: JSON.stringify(item.DistrictCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getDistrictList();
                }
            });
        } else {
            return false;
        }
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/District/ChangeStatusDistrict",
                params: {
                    code: JSON.stringify(item.DistrictCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getDistrictList();
                }
            });
        } else {
            return false;
        }
    };

    function getDistrictList() {
        var model = {
            DistrictName: $('#district-name').val(),
            Status: $('#status').val(),
            DistrictCode: $('#district-code').val(),
            CityCode: $('#city-code').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/District/GetListDistrict",
            params: {
                name: JSON.stringify(model.DistrictName),
                status: JSON.stringify(model.Status),
                code: JSON.stringify(model.DistrictCode),
                citycode: JSON.stringify(model.CityCode)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("branchController", function ($scope, $http, $timeout)
{
    getBranchList();
    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "branch-code" || e.target.id === "branch-name" ||
            e.target.id === "city" || e.target.id === "district" || e.target.id === "status"))
        {
            getBranchList();
        }
    });
    $scope.branchSearch = function ()
    {
        getBranchList();
    };
    $scope.changeStatus = function (item)
    {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Branch/ChangeStatusBranch",
                params: {
                    code: JSON.stringify(item.BranchCode)
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else
                {
                    alert(msg.data.toString());
                    getBranchList();
                }
            });
        } else
        {
            return false;
        }
    };

    function getBranchList()
    {
        var model = {
            BranchCode: $('#branch-code').val(),
            BranchName: $('#branch-name').val(),
            CityCode: $('#city').val(),
            DistrictCode: $('#district').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Branch/GetListBranch",
            params: {
                code: JSON.stringify(model.BranchCode),
                name: JSON.stringify(model.BranchName),
                city: JSON.stringify(model.CityCode),
                district: JSON.stringify(model.DistrictCode),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function ()
        {
            $timeout(function ()
            {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("benebranchController", function ($scope, $http, $timeout) {
    getBranchList();
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "bank" || e.target.id === "branch-code" || e.target.id === "branch-name" ||
            e.target.id === "city" || e.target.id === "district" || e.target.id === "status")) {
            getBranchList();
        }
    });
    $scope.branchSearch = function () {
        getBranchList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/BeneBranch/ChangeStatusBranch",
                params: {
                    code: JSON.stringify(item.BranchCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getBranchList();
                }
            });
        } else {
            return false;
        }
    };

    function getBranchList() {
        var model = {
            BranchCode: $('#branch-code').val(),
            BranchName: $('#branch-name').val(),
            CityCode: $('#city').val(),
            DistrictCode: $('#district').val(),
            Bank: $('#bank').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/BeneBranch/GetListBranch",
            params: {
                code: JSON.stringify(model.BranchCode),
                name: JSON.stringify(model.BranchName),
                city: JSON.stringify(model.CityCode),
                district: JSON.stringify(model.DistrictCode),
                status: JSON.stringify(model.Status),
                bank: JSON.stringify(model.Bank),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("posTypeController", function ($scope, $http, $timeout) {
    getListPosType();
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "postype-code" || e.target.id === "postype-name") || e.target.id === "status") {
            getListPosType();
        }
    });
    $scope.posTypeSearch = function () {
        getListPosType();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/PosType/ChangeStatusPosType",
                params: {
                    code: JSON.stringify(item.PosTypeCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListPosType();
                }
            });
        } else {
            return false;
        }
    };

    function getListPosType() {
        var model = {
            PosTypeCode: $('#postype-code').val(),
            PosTypeName: $('#postype-name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/PosType/GetListPosType",
            params: {
                code: JSON.stringify(model.PosTypeCode),
                name: JSON.stringify(model.PosTypeName),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; 
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("posController", function ($scope, $http) {
    getLisPos();
    $scope.posSearch = function () {
        getLisPos();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Pos/ChangeStatusPos",
                params: {
                    code: JSON.stringify(item.PosCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getLisPos();
                }
            });
        } else {
            return false;
        }
    };
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "pos-code" || e.target.id === "pos-name") || e.target.id === "branch-code"
            || e.target.id === "status" || e.target.id === "postype-code") {
            getLisPos();
        }
    });
    function getLisPos() {
        var model = {
            PosCode: $('#pos-code').val(),
            PosName: $('#pos-name').val(),
            BranchCode: $('#branch-code').val(),
            PosTypeCode: $('#postype-code').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Pos/GetListPos",
            params: {
                code: JSON.stringify(model.PosCode),
                name: JSON.stringify(model.PosName),
                branch: JSON.stringify(model.BranchCode),
                postype: JSON.stringify(model.PosTypeCode),
                status: JSON.stringify(model.Status)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("posDetailController", function ($scope, $http) {
    getLisPosDetail();
    $scope.posDetailSearch = function () {
        getLisPosDetail();
    };
    $scope.pageChanged = function () {
        getLisPosDetail();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/PosDetail/ChangeStatusPosDetail",
                params: {
                    id: JSON.stringify(item.PosDetailId)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getLisPosDetail();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "code" ||  e.target.id === "posDetail-name" || e.target.id === "branch-code" || e.target.id === "city-code"
            || e.target.id === "status" || e.target.id === "district-code" || e.target.id === "postype-code")) {
            getLisPosDetail();
        }
    });

    function getLisPosDetail() {
        var model = {
            PosDetailID: $('#code').val(),
            PosDetailName: $('#posDetail-name').val(),
            BranchCode: $('#branch-code').val(),
            CityCode: $('#city-code').val(),
            DistrictCode: $('#district-code').val(),
            PosTypeCode: $('#postype-code').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/PosDetail/GetListPosDetail",
            params: {
                code: JSON.stringify(model.PosDetailID),
                name: JSON.stringify(model.PosDetailName),
                branch: JSON.stringify(model.BranchCode),
                city: JSON.stringify(model.CityCode),
                district: JSON.stringify(model.DistrictCode),
                postype: JSON.stringify(model.PosTypeCode),
                status: JSON.stringify(model.Status)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("bankController", function ($scope, $http, $timeout) {
    getBankList();

    $scope.bankSearch = function () {
        getBankList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Bank/ChangeStatusBank",
                params: {
                    bankCode: JSON.stringify(item.BankCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = "/Error/Index";
                } else {
                    //document.getElementById("lblMessage").innerText = msg.data.toString();
                    //document.getElementById("lblMessage").style.color = "blue";
                    alert(msg.data.toString());
                    getBankList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "bank-code" || e.target.id === "bank-name" ||
             e.target.id === "status")) {
            getBankList();
        }
    });

    function getBankList() {
        var model = {
            BankCode: $('#bank-code').val(),
            BankName: $('#bank-name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Bank/GetLisBanks",
            params: {
                bankCode: JSON.stringify(model.BankCode),
                bankName: JSON.stringify(model.BankName),
                bankStatus: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("customerbankController", function ($scope, $http, $timeout) {       
    $scope.customerNext = function () {
        var models = [];
        angular.forEach($scope.filtered, function (item) {
            if (item.Selected == true) {               
                models.push(item);               
            }
        });
        if (models.length <= 0 || models.length >=2) {            
            alert("You must select one record to browse.");
            return false;
        }        
        $http.post(getRootUrl() + '/Transfer/GetView', models)
         .then(function (data) {
             //if (redirectToLogin(data)) {
             //    window.location.href=  getRootUrl() + "/Error/Index";
             //} else {
             window.location.href = getRootUrl()+"/Transfer/NewTransfer";
             //}
         });       
    };
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "cusname" || e.target.id === "id-pass" || e.target.id === "account" || e.target.id === "cif")) {
            getCustomerList();
        }
    });   
    $scope.customerSearch = function () {       
        var model = {
            Account: $('#account').val(),
            Cif: $('#cif').val()
        };
        if (model.Account == "" && model.Cif == "")
        {
            location.reload(true);
        }
        $('#progess').modal('show');
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Transfer/GetListCustomer",
            params: {
                account: JSON.stringify(model.Account),
                cif: JSON.stringify(model.Cif),
            }
        });
        datas.success(function (data) {
            $('#progess').modal('hide');
            if (data.toString().indexOf("!DOCTYPE") > 0) {
                window.location.href = getRootUrl() + "/Error/Index";
            }        
            else if (typeof data == "string" && data != " ") {
                alert(data);
        }
            else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;                
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.               
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;                
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
        
    }
});
systemSearchApp.controller("currencyController", function ($scope, $http, $timeout)
{
    getCurrencyList();

    $scope.currencySearch = function ()
    {
        getCurrencyList();
    };

    $scope.changeStatus = function (item)
    {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Currency/ChangeStatusCurrency",
                params: {
                    code: JSON.stringify(item.CurrencyCode)
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else
                {
                    alert(msg.data.toString());
                    getCurrencyList();
                }
            });
        } else
        {
            return false;
        }
    };

    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status"))
        {
            getCurrencyList();
        }
    });

    function getCurrencyList()
    {
        var model = {
            CurrencyCode: $('#currency-code').val(),
            CurrencyName: $('#currency-name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Currency/GetListCurrency",
            params: {
                code: JSON.stringify(model.CurrencyCode),
                name: JSON.stringify(model.CurrencyName),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function ()
        {
            $timeout(function ()
            {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("billserviceController", function ($scope, $http, $timeout) {
    getBillServiceList();

    $scope.billserviceSearch = function () {
        getBillServiceList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/BillService/ChangeStatusBillService",
                params: {
                    code: JSON.stringify(item.BillServiceCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getBillServiceList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status")) {
            getCurrencyList();
        }
    });

    function getBillServiceList() {
        var model = {
            BillServiceName: $('#billprovider-name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/BillService/GetListBillService",
            params: {
                name: JSON.stringify(model.BillServiceName),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("billproviderController", function ($scope, $http, $timeout) {
    getBillProviderList();

    $scope.billproviderSearch = function () {
        getBillProviderList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/BillProvider/ChangeStatusBillProvider",
                params: {
                    code: JSON.stringify(item.BillProviderCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getBillProviderList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "currency-code" || e.target.id === "currency-name" || e.target.id === "status")) {
            getBillProviderList();
        }
    });

    function getBillProviderList() {
        var model = {
          
            ServiceTypeCode: $('#servicetype').val(),
            BillProviderName: $('#billprovider-name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/BillProvider/GetListBillProvider",
            params: {
                serviceType: JSON.stringify(model.ServiceTypeCode),
                name: JSON.stringify(model.BillProviderName),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("configCusTypeController", function ($scope, $http, $timeout) {
    getListConfig();

    $scope.configCusTypeSearch = function () {
        getListConfig();
    };
    $scope.delete = function (item) {
        var confirmMesg = "Bạn muốn xóa bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/ConfigurationCusType/Delete",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListConfig();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && e.target.id === "cus-type") {
            getListConfig();
        }
    });

    function getListConfig() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/ConfigurationCusType/GetList",
            params: {
                custype: JSON.stringify($('#cus-type').val()),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10;
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("telecomController", function ($scope, $http, $timeout) {
    getTelecomList();

    $scope.telecomSearch = function () {
        getTelecomList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Telecom/ChangeStatusTelecom",
                params: {
                    code: JSON.stringify(item.TelecomCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getTelecomList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "telecom-code" || e.target.id === "telecom-name" || e.target.id === "status")) {
            getTelecomList();
        }
    });

    function getTelecomList() {
        var model = {
            TelecomCode: $('#telecom-code').val(),
            TelecomName: $('#telecom-name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Telecom/GetListTelecom",
            params: {
                code: JSON.stringify(model.TelecomCode),
                name: JSON.stringify(model.TelecomName),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});


systemSearchApp.controller("telNumberController", function ($scope, $http, $timeout) {
    getTelNumberList();

    $scope.telNumberSearch = function () {
        getTelNumberList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/TelNumber/ChangeStatusTelNumber",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getTelNumberList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "telcom-code" || e.target.id === "fromNumber" || e.target.id === "status" || e.target.id === "toNumber")) {
            getTelNumberList();
        }
    });

    function getTelNumberList() {
        var model = {
            TelcoCode: $('#telcom-code').val(),
            FromNumber: $('#fromNumber').val(),
            ToNumber: $('#toNumber').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/TelNumber/GetListTelNumber",
            params: {
                telcode: JSON.stringify(model.TelcoCode),
                fromnumber: JSON.stringify(model.FromNumber),
                tonumber: JSON.stringify(model.ToNumber),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("configController", function ($scope, $http, $timeout)
{
    getListConfig();

    $scope.configSearch = function ()
    {
        getListConfig();
    };  
    $scope.changeStatus = function (item)
    {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true)
        {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Configuration/ChangeStatus",
                params: {
                    code: JSON.stringify(item.Code),
                    currentStatus: JSON.stringify(item.StatusId)
                }
            });
            response.then(function (msg)
            {
                if (redirectToLogin(msg))
                {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else
                {
                    alert(msg.data.toString());
                    getListConfig();
                }
            });
        } else
        {
            return false;
        }
    };
    $scope.configReset = function (item) {
        var confirmMesg = "Do you want to Reset Cache?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Configuration/ResetCache",
                params: {
                    
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListConfig();
                }
            });
        } else {
            return false;
        }
    };

    $scope.configResetAll = function (item) {
        var confirmMesg = "Do you want to Reset Cache?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Configuration/ResetCacheAll",
                params: {
                    
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListConfig();
                }
            });
        } else {
            return false;
        }
    };
    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "code" || e.target.id === "value" || e.target.id === "desc" || e.target.id === "status"))
        {
            getListConfig();
        }
    });

    function getListConfig()
    {
        var model = {
            Code: $('#code').val(),
            Value: $('#value').val(),
            Desc: $('#desc').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Configuration/GetList",
            params: {
                code: JSON.stringify(model.Code),
                value: JSON.stringify(model.Value),
                desc: JSON.stringify(model.Desc),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10;
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function ()
        {
            $timeout(function ()
            {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("serviceTypeController", function ($scope, $http, $timeout) {
    getServiceTypeList();

    $scope.serviceTypeSearch = function () {
        getServiceTypeList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/ServiceType/ChangeStatusServiceType",
                params: {
                    code: JSON.stringify(item.ServiceTypeCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getServiceTypeList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "code" || e.target.id === "name") || e.target.id === "status") {
            getServiceTypeList();
        }
    });

    function getServiceTypeList() {
        var model = {
            ServiceTypeCode: $('#code').val(),
            ServiceTypeName: $('#name').val(),
            Status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/ServiceType/GetListServiceType",
            params: {
                code: JSON.stringify(model.ServiceTypeCode),
                name: JSON.stringify(model.ServiceTypeName),
                status: JSON.stringify(model.Status),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("serviceController", function ($scope, $http, $timeout) {
    getServiceList();

    $scope.serviceSearch = function () {
        getServiceList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/ServiceMB/ChangeStatusService",
                params: {
                    code: JSON.stringify(item.ServiceCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getServiceList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "code" || e.target.id === "name" || e.target.id === "status" || e.target.id === "servicetype")) {
            getServiceList();
        }
    });

    function getServiceList() {
        var model = {
            ServiceCode: $('#code').val(),
            ServiceName: $('#name').val(),
            Status: $('#status').val(),
            ServiceType: $('#servicetype').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/ServiceMB/GetListService",
            params: {
                code: JSON.stringify(model.ServiceCode),
                name: JSON.stringify(model.ServiceName),
                status: JSON.stringify(model.Status),
                servicetypecode: JSON.stringify(model.ServiceType),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("packageController", function ($scope, $http, $timeout) {
    getPackageList();

    $scope.packageSearch = function () {
        getPackageList();
    };

    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/ChangeStatusPackage",
                params: {
                    code: JSON.stringify(item.PackageCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getPackageList();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "code" || e.target.id === "name" || e.target.id === "status")) {
            getPackageList();
        }
    });
    $scope.delete = function (item) {
        var confirmMesg = "Do you want to Delete this package?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/Delete",
                params: {
                    code: JSON.stringify(item.PackageCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    if (msg.data.toString().length > 0) {
                        alert(msg.data.toString());
                        return false;
                    }
                    alert("Delete this package is scuccessfully");
                    getPackageList();
                }
            });
        } else {
            return false;
        }
    };
    $scope.viewDetail = function (item) {
        $http({
            method: "post",
            url: getRootUrl() + "/Package/ViewDetail",
            params: {

                code: JSON.stringify(item.PackageCode),
            }
        }).then(function (msg) {

            if (redirectToLogin(msg)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                var model = msg.data;
                document.getElementById('info-code').innerHTML = model.PackageCode;
                document.getElementById('info-name').innerHTML = model.PackageName;
                document.getElementById('info-servicechannel').innerHTML = model.ServiceChannel;
                document.getElementById('info-monthfee').innerHTML = displayCurrency(model.MonthFee);
                document.getElementById('info-vat').innerHTML = displayCurrency(model.VAT);
                document.getElementById('info-resident').innerHTML = model.Resident;
                document.getElementById('info-amount').innerHTML = displayCurrency(model.AmountTotal);
                $('#service-fee tr').has('td').remove();
                $('#service-limit tr').has('td').remove();
                if (model.PackageLimitList.length > 0) {
                    for (var i = 0; i < model.PackageLimitList.length; i++) {
                        var tr = document.getElementById('service-limit').insertRow(i + 1);
                        var tdService = tr.insertCell(0);
                        tdService.innerHTML = model.PackageLimitList[i].ServiceName;
                        var tdTimesDay = tr.insertCell(1);
                        tdTimesDay.innerHTML = displayCurrency(model.PackageLimitList[i].TimesDay);
                        var tdMinTran = tr.insertCell(2);
                        tdMinTran.innerHTML = displayCurrency(model.PackageLimitList[i].TranMin);
                        var tdMaxTran = tr.insertCell(3);
                        tdMaxTran.innerHTML = displayCurrency(model.PackageLimitList[i].TranMax);
                        var tdTotal = tr.insertCell(4);
                        tdTotal.innerHTML = displayCurrency(model.PackageLimitList[i].TotalLimit);
                        var tdStatus = tr.insertCell(5);
                        tdStatus.innerHTML = model.PackageLimitList[i].Status;
                    }
                }
                if (model.PackageServiceList.length > 0) {
                    for (var i = 0; i < model.PackageServiceList.length; i++) {
                        var tr = document.getElementById('service-fee').insertRow(i + 1);
                        var tdServicesv = tr.insertCell(0);
                        tdServicesv.innerHTML = model.PackageServiceList[i].ServiceName;
                        var tdFeeType = tr.insertCell(1);
                        tdFeeType.innerHTML = model.PackageServiceList[i].FeeType;
                        var tdFlat = tr.insertCell(2);
                        tdFlat.innerHTML = displayCurrency(model.PackageServiceList[i].FlatPercent);
                        var tdVat = tr.insertCell(3);
                        tdVat.innerHTML = displayCurrency(model.PackageServiceList[i].VAT);
                        var tdMinFee = tr.insertCell(4);
                        tdMinFee.innerHTML = displayCurrency(model.PackageServiceList[i].MinFee);
                        var tdMaxFee = tr.insertCell(5);
                        tdMaxFee.innerHTML = displayCurrency(model.PackageServiceList[i].MaxFee);
                        var tdFromAmount = tr.insertCell(6);
                        tdFromAmount.innerHTML = displayCurrency(model.PackageServiceList[i].FromAmount);
                        var tdToAmount = tr.insertCell(7);
                        tdToAmount.innerHTML = displayCurrency(model.PackageServiceList[i].ToAmount);
                        var tdFromHour = tr.insertCell(8);
                        tdFromHour.innerHTML = displayCurrency(model.PackageServiceList[i].FromHour);
                        var tdToHour = tr.insertCell(9);
                        tdToHour.innerHTML = displayCurrency(model.PackageServiceList[i].ToHour);
                        var tdFee = tr.insertCell(10);
                        tdFee.innerHTML = displayCurrency(model.PackageServiceList[i].Fee);
                        var tdStatusSc = tr.insertCell(11);
                        tdStatusSc.innerHTML = model.PackageServiceList[i].Status;
                    }
                }
                $('#viewDetail').modal("show");
            }
        });
    };
    function getPackageList() {
        var model = {
            Code: $('#code').val(),
            Name: $('#name').val(),
            Status: $('#status').val(),
            Role: $('#role').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Package/GetListPackage",
            params: {
                code: JSON.stringify(model.Code),
                name: JSON.stringify(model.Name),
                status: JSON.stringify(model.Status),
                role: JSON.stringify(model.Role),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("serviceLimitController", function ($scope, $http, $timeout) {

    getListServiceLimit();
    $scope.addServiceLimit = function () {
        $('#times-day').val("");
        $('#min-tran').val("");
        $('#max-tran').val("");
        $('#total-limit').val("");
        $('#add-new-servicelimit').modal("show");
    };
    $scope.editServiceLimit = function (item) {
        $('#serviceLimitId').val(item.Id);
        $('#edit-service').val(item.ServiceCode);
        $('#edit-times-day').val(displayCurrency(item.TimesDay));
        $('#edit-min-tran').val(displayCurrency(item.TranMin));
        $('#edit-max-tran').val(displayCurrency(item.TranMax));
        $('#edit-total-limit').val(displayCurrency(item.TotalLimit));
        $('#edit-currency').val(item.CurrencyCode);
        $('#edit-servicelimit').modal("show");
    };
    $scope.save = function () {
        if (parseInt($('#min-tran').val().replace(/,/g, "")) > parseInt($('#max-tran').val().replace(/,/g, ""))) {
            alert("Max limit/Trans cannot be less than Min limit/ Trans");
            return false;
        }
        var model = {
            PackageCode: $('#pkg-code').val(),
            ServiceCode: $('#service').val(),
            TimesDay: $('#times-day').val(),
            TranMin: $('#min-tran').val(),
            TranMax: $('#max-tran').val(),
            TotalLimit: $('#total-limit').val(),
            Currency: $('#currency').val()
        };
        $http.post(getRootUrl() + '/Package/CreateServiceLimit', model)
            .then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    $('#add-new-servicelimit').modal("hide");
                    window.location.reload();
                }
            });
    };
    $scope.update = function () {
        if (parseInt($('#edit-min-tran').val().replace(/,/g, "")) > parseInt($('#edit-max-tran').val().replace(/,/g, ""))) {
            alert("Max limit/Trans cannot be less than Min limit/ Trans");
            return false;
        }
        var model = {
            Id: $('#serviceLimitId').val(),
            ServiceCode: $('#edit-service').val(),
            TimesDay: $('#edit-times-day').val(),
            TranMin: $('#edit-min-tran').val(),
            TranMax: $('#edit-max-tran').val(),
            TotalLimit: $('#edit-total-limit').val(),
            Currency: $('#edit-currency').val()
        };
        $http.post(getRootUrl() + '/Package/UpdateServiceLimit', model)
           .then(function (msg) {
               if (redirectToLogin(msg)) {
                   window.location.href = getRootUrl() + "/Error/Index";
               } else {
                   alert(msg.data.toString());
                   $('#edit-servicelimit').modal("hide");
                   window.location.reload();
               }
           });
    }
    $scope.changeStatus = function (item) {
        var confirmMesg;
        if (item.Status) confirmMesg = "Do you want to lock this service limit?";
        else {
            confirmMesg = "Do you want to unlock this service limit?";
        }
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/ChangeStatusServiceLimit",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListServiceLimit();
                }
            });
        } else {
            return false;
        }
    };
    $scope.deleteLimit = function (item) {
        var confirmMesg = "Do you want to Delete this service limit?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/DeleteServiceLimit",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListServiceLimit();
                }
            });
        } else {
            return false;
        }
    };

    function getListServiceLimit() {
        var model = {
            Code: $('#pkg-code').val(),
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Package/GetListServiceLimit",
            params: {
                pkgCode: JSON.stringify(model.Code),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("packageLimitController", function ($scope, $http, $timeout) {

    getListPackageLimit();
    $scope.addPackageLimit = function () {
        $('#total-limit').val("");
        $('#add-new-packagelimit').modal("show");
    };
    $scope.editPackageLimit = function (item) {
        $('#pkgLimitId').val(item.Id);
        $('#edit-total-limit').val(displayCurrency(item.TotalLimit));
        $('#edit-currency').val(item.CurrencyCode);
        $('#edit-packagelimit').modal("show");
    };
    $scope.save = function () {
        var model = {
            PackageCode: $('#pkg-code').val(),
            TotalLimit: $('#total-limit').val(),
            Currency: $('#currency').val()
        };
        $http.post(getRootUrl() + '/Package/CreateToatlLimit', model)
            .then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    $('#add-new-packagelimit').modal("hide");
                    window.location.reload();
                }
            });
    };
    $scope.update = function () {        
        var model = {
            Id: $('#pkgLimitId').val(),
            TotalLimit: $('#edit-total-limit').val(),
            Currency: $('#edit-currency').val()
        };
        $http.post(getRootUrl() + '/Package/UpdateTotalLimit', model)
           .then(function (msg) {
               if (redirectToLogin(msg)) {
                   window.location.href = getRootUrl() + "/Error/Index";
               } else {
                   alert(msg.data.toString());
                   $('#edit-packagelimit').modal("hide");
                   window.location.reload();
               }
           });
    }
    $scope.deletepkgLimit = function (item) {
        var confirmMesg = "Do you want to Delete this Package limit?";       
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/DeleteTotalLimit",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListPackageLimit();
                }
            });
        } else {
            return false;
        }
    };

    function getListPackageLimit() {
        var model = {
            Code: $('#pkg-code').val(),
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Package/GetListTotalLimit",
            params: {
                pkgCode: JSON.stringify(model.Code),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("serviceFeeController", function ($scope, $http, $timeout) {
    getListServiceFee();
    function isMobile(mobile) {
        var mobileNo = /^[0-9]\d*(\.\d{1,2})?$/;
        if (mobileNo.test(mobile))
            return true;
        else
            return false;
    }
    $scope.addServiceFee = function () {
        $('#vat').val("");
        $('#min-fee').val("");
        $('#max-fee').val("");
        $('#from-amount').val("");
        $('#to-amount').val("");
        $('#fee').val("");
        $('#add-new-servicefee').modal("show");
    };
    $scope.editServiceFee = function (item) {
        $('#serviceFeeId').val(item.Id);
        $('#edit-service').val(item.ServiceCode);
        $('#edit-area-fee').val(item.Areafee == "Same" ? "0" : "1");
        $('#edit-currency').val(item.CurrencyCode);
        $('#edit-fee-type').val(item.FeeType == "Flat" ? "0" : "1");
        $('#edit-vat').val(item.VAT);
        $('#edit-min-fee').val(displayCurrency(item.MinFee));
        $('#edit-max-fee').val(displayCurrency(item.MaxFee));
        $('#edit-from-amount').val(displayCurrency(item.FromAmount));
        $('#edit-to-amount').val(displayCurrency(item.ToAmount));
        $('#edit-fee').val(displayCurrency(item.Fee));
        if ($('#edit-fee-type').val() == "0") {
            $("#edit-min-fee").prop('disabled', true);
            $("#edit-max-fee").prop('disabled', true);
        }
        else {
            $("#edit-min-fee").prop('disabled', false);
            $("#edit-max-fee").prop('disabled', false);
        }
        $('#edit-servicefee').modal("show");
    };
    $scope.save = function () {
        var model = {
            PackageCode: $('#pkg-code').val(),
            ServiceCode: $('#service').val(),
            Areafee: $('#area-fee').val(),
            Currency: $('#currency').val(),
            FeeType: $('#fee-type').val(),
            ToAmount: $('#to-amount').val(),
            FromAmount: $('#from-amount').val(),
            VAT: $('#vat').val(),
            MinFee: $('#min-fee').val(),
            MaxFee: $('#max-fee').val(),
            Fee: $('#fee').val()
        };        
        if (!isMobile($('#fee').val().replace(/,/g, ""))) {
            alert("Fee is invalid, number only or Maximum is 2 decimals, please check again.");
            $('#fee').focus();
            return false;
        }       
        if ($('#fee-type').val() == "1") {
            if (!isMobile($('#min-fee').val().replace(/,/g, ""))) {
                alert("Min Fee is invalid, number only or Maximum is 2 decimals, please check again.");
                $('#min-fee').focus();
                return false;
            }
            if (!isMobile($('#max-fee').val())) {
                alert("Max Fee is invalid, number only or Maximum is 2 decimals, please check again.");
                $('#max-fee').focus();
                return false;
            }
            if (parseInt(model.MinFee.replace(/,/g, "")) > parseInt(model.MaxFee.replace(/,/g, ""))) {
                alert("MaxFee cannot be less than MinFee");
                return false;
            }
        }
        if (parseInt(model.FromAmount.replace(/,/g, "")) > parseInt(model.ToAmount.replace(/,/g, ""))) {
            alert("ToAmount cannot be less than FromAmount");
            return false;
        }
        if (parseFloat(model.Fee.replace(/,/g, "")) <= 0) {
            alert("Fee must be larger than 0, please check back");
            return false;
        }
        $http.post(getRootUrl() + '/Package/CreateServiceFee', model)
            .then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    $('#add-new-servicefee').modal("hide");
                    getListServiceFee();
                }
            });
    };
    $scope.update = function () {
        var model = {
            Id: $('#serviceFeeId').val(),
            PackageCode: $('#pkg-code').val(),
            ServiceCode: $('#edit-service').val(),
            Areafee: $('#edit-area-fee').val(),
            Currency: $('#edit-currency').val(),
            FeeType: $('#edit-fee-type').val(),
            ToAmount: $('#edit-to-amount').val(),
            FromAmount: $('#edit-from-amount').val(),
            VAT: $('#edit-vat').val(),
            MinFee: $('#edit-min-fee').val(),
            MaxFee: $('#edit-max-fee').val(),
            Fee: $('#edit-fee').val()
        };
        if (!isMobile($('#edit-fee').val().replace(/,/g, ""))) {
            alert("Fee is invalid, number only or Maximum is 2 decimals, please check again.");
            $('#edit-fee').focus();
            return false;
        }
        if ($('#edit-fee-type').val() == "1") {
            if (!isMobile($('#edit-min-fee').val())) {
                alert("Min Fee is invalid, number only or Maximum is 2 decimals, please check again.");
                $('#edit-min-fee').focus();
                return false;
            }
            if (!isMobile($('#edit-max-fee').val().replace(/,/g, ""))) {
                alert("Max Fee is invalid, number only or Maximum is 2 decimals, please check again.");
                $('#edit-max-fee').focus();
                return false;
            }
        }
        if (parseInt(model.MinFee.replace(/,/g, "")) > parseInt(model.MaxFee.replace(/,/g, ""))) {
            alert("MaxFee cannot be less than MinFee");
            return false;
        }
        if (parseInt(model.FromAmount.replace(/,/g, "")) > parseInt(model.ToAmount.replace(/,/g, ""))) {
            alert("ToAmount cannot be less than FromAmount");
            return false;
        }
        if (parseFloat(model.Fee.replace(/,/g, "")) <= 0) {
            alert("Fee must be larger than 0, please check back");
            return false;
        }
        $http.post(getRootUrl() + '/Package/UpdateServiceFee', model)
           .then(function (msg) {
               if (redirectToLogin(msg)) {
                   window.location.href = getRootUrl() + "/Error/Index";
               } else {
                   alert(msg.data.toString());
                   $('#edit-servicefee').modal("hide");
                   getListServiceFee();
               }
           });
    }
    $scope.changeStatus = function (item) {
        var confirmMesg;
        if (item.Status) confirmMesg = "Do you want to lock this service fee?";
        else {
            confirmMesg = "Do you want to unlock this service fee?";
        }
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/ChangeStatusServiceFee",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListServiceFee();
                }
            });
        } else {
            return false;
        }
    };
    $scope.deleteFee = function (item) {
        var confirmMesg = "Do you want to Delete this service fee?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/DeleteServiceFee",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListServiceFee();
                }
            });
        } else {
            return false;
        }
    };
    function getListServiceFee() {
        var model = {
            Code: $('#pkg-code').val(),
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Package/GetListServiceFee",
            params: {
                pkgCode: JSON.stringify(model.Code),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("packageServiceController", function ($scope, $http, $timeout) {

    getListPackageService();
    $scope.addPackageService = function () {
        $('#add-new-package_service').modal("show");
    };
    $scope.editPackageService = function (item) {
        $('#pkgServiceId').val(item.Id);
        $('#edit-service').val(item.ServiceCode);
        $('#edit-package_service').modal("show");
    };
    $scope.save = function () {
        var model = {
            PackageCode: $('#pkg-code').val(),
            ServiceCode: $('#service').val()
        };
        $http.post(getRootUrl() + '/Package/CreateService', model)
            .then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    $('#add-new-package_service').modal("hide");
                    window.location.reload();
                }
            });
    };
    $scope.update = function () {
        var model = {
            Id: $('#pkgServiceId').val(),
            ServiceCode: $('#edit-service').val()
        };
        $http.post(getRootUrl() + '/Package/UpdateService', model)
           .then(function (msg) {
               if (redirectToLogin(msg)) {
                   window.location.href = getRootUrl() + "/Error/Index";
               } else {
                   alert(msg.data.toString());
                   $('#edit-package_service').modal("hide");
                   window.location.reload();
               }
           });
    }
    $scope.deletepkgService = function (item) {
        var confirmMesg = "Do you want to Delete this Package Service?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/DeleteService",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListPackageService();
                }
            });
        } else {
            return false;
        }
    };

    function getListPackageService() {
        var model = {
            Code: $('#pkg-code').val(),
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Package/GetListService",
            params: {
                pkgCode: JSON.stringify(model.Code),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("packageFeeController", function ($scope, $http, $timeout) {
    getListPackageFee();
    function isMobile(mobile) {
        var mobileNo = /^[0-9]\d*(\.\d{1,2})?$/;
        if (mobileNo.test(mobile))
            return true;
        else
            return false;
    }
    $scope.addPackageFee = function () {
        $('#vat').val("");
        $('#fee').val("");
        $('#add-new-packagefee').modal("show");
    };
    $scope.editPackageFee = function (item) {
        $('#packageFeeCode').val(item.Id);
        $('#edit-currency').val(item.CurrencyCode);
        $('#edit-period').val(item.Period == "Yearly" ? "YEAR" : "");
        $('#edit-vat').val(item.VAT);
        $('#edit-fee').val(displayCurrency(item.Fee));       
        $('#edit-packagefee').modal("show");
    };
    $scope.save = function () {
        var model = {
            PackageCode: $('#pkg-code').val(),
            Currency: $('#currency').val(),
            Period: $('#period').val(),
            VAT: $('#vat').val(),
            Fee: $('#fee').val()
        };
        if (!isMobile($('#fee').val().replace(/,/g, ""))) {
            alert("Fee is invalid, number only or Maximum is 2 decimals, please check again.");
            $('#fee').focus();
            return false;
        }        
        if (parseFloat(model.Fee.replace(/,/g, "")) <= 0) {
            alert("Fee must be larger than 0, please check back");
            return false;
        }
        if ($('#vat').val().replace(/,/g, "") != "") {
            if (!isMobile($('#vat').val().replace(/,/g, ""))) {
                alert("VAT is invalid, number only or Maximum is 2 decimals, please check again.");
                $('#vat').focus();
                return false;
            }
        }
        if (parseFloat(model.VAT.replace(/,/g, "")) <= 0 || parseFloat(model.VAT.replace(/,/g, "")) > 100) {
            alert("Vat must be larger than 0 and less 100 , please check back");
            return false;
        }
        $http.post(getRootUrl() + '/Package/CreatePackageFee', model)
            .then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    $('#add-new-packagefee').modal("hide");
                    getListPackageFee();
                }
            });
    };
    $scope.update = function () {
        var model = {
            Id: $('#packageFeeCode').val(),
            PackageCode: $('#pkg-code').val(),
            Currency: $('#edit-currency').val(),
            Period: $('#edit-period').val(),
            VAT: $('#edit-vat').val(),
            Fee: $('#edit-fee').val()
        };
        if (!isMobile($('#edit-fee').val().replace(/,/g, ""))) {
            alert("Fee is invalid, number only or Maximum is 2 decimals, please check again.");
            $('#edit-fee').focus();
            return false;
        }       
        if (parseFloat(model.Fee.replace(/,/g, "")) <= 0) {
            alert("Fee must be larger than 0, please check back");
            return false;
        }
        if ($('#edit-vat').val().replace(/,/g, "") != "") {
            if (!isMobile($('#edit-vat').val().replace(/,/g, ""))) {
                alert("VAT is invalid, number only or Maximum is 2 decimals, please check again.");
                $('#edit-vat').focus();
                return false;
            }
        }
        if (parseFloat(model.VAT.replace(/,/g, "")) <= 0 || parseFloat(model.VAT.replace(/,/g, "")) > 100) {
            alert("Vat must be larger than 0 and less 100 , please check back");
            return false;
        }
        $http.post(getRootUrl() + '/Package/UpdatePackageFee', model)
           .then(function (msg) {
               if (redirectToLogin(msg)) {
                   window.location.href = getRootUrl() + "/Error/Index";
               } else {
                   alert(msg.data.toString());
                   $('#edit-packagefee').modal("hide");
                   getListPackageFee();
               }
           });
    }
    $scope.deletePackageFee = function (item) {
        var confirmMesg = "Are you want to delete the record?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Package/DeletePackageFee",
                params: {
                    id: JSON.stringify(item.Id)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListPackageFee();
                }
            });
        } else {
            return false;
        }
    };
    function getListPackageFee() {
        var model = {
            Code: $('#pkg-code').val(),
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Package/GetListPackageFee",
            params: {
                pkgCode: JSON.stringify(model.Code),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller('customerController', function ($scope, $http) {
    var idCustomer = 0;
    var liAcount;
    $(document).keypress(function (e) {
        if (e.which === 13 && (e.target.id === "account" || e.target.id === "cif" || e.target.id === "id-passport")) {
            searchCustomer();
        }
    });
    $scope.searchCustomer = function () {
        var cif = $('#cif').val();
        //Check dữ liệu tìm kiếm
        var idNumber = $('#id-passport').val();
        if (idNumber === "" && cif === "") {
            document.getElementById('notify-customer').innerHTML = "Please enter cif or id-passport!";
            document.getElementById('notify-customer').style.color = "red";
            return false;
        } else {
            document.getElementById('notify-customer').innerHTML = "";
        }
        searchCustomer();
    };

    $scope.createCustomer = function () {
        
        if ($('#notify-result').hasClass('red')) {
            $('#notify-result').removeClass('red');
        }
        if ($('#cus-birth').val() === "") {
            document.getElementById("warn-birthday").innerHTML = "Please enter Birthday!";
            return false;
        } else {
            document.getElementById("warn-birthday").innerHTML = "";
        }
        //Check địa chỉ
        if ($('#cus-address').val() === "") {
            document.getElementById("warn-address").innerHTML = "Please enter address!";
            return false;
        } else {
            document.getElementById("warn-address").innerHTML = "";
        }
        //Check Customer Type
        if ($('#cus-type option:selected').text() === "") {
            document.getElementById("warn-custype").innerHTML = "Please enter Customer Type!";
            return false;
        } else {
            document.getElementById("warn-custype").innerHTML = "";
        }
        if ($('#cus-email').val() != "") {
            var pw = $('#cus-email').val();
            var pattern = /\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
            var check = pattern.test(pw);
            if (!check) {
                document.getElementById("warn-email").innerHTML = "Invalid Email. Please check!!";
                //alert("Invalid Email. Please check!");
                return false;
            }
            else {
                document.getElementById("warn-email").innerHTML = "";
            }
        }
        //Check chọn tài khoản default
        var demcheck = 0;
        var acountDefault;
        $("input[type=radio]").each(function () {
            var id1 = $(this).attr("id");
            if (document.getElementById(id1).checked === true) {
                demcheck++;
                acountDefault = id1;
            }
        });
        if (demcheck === 0) {
            alert("Please choose Default account!");
            return false;
        }
        
        //Check nhập username  
            var telname = $('#tel-name').val();
            if (telname.length <= 0) {
                document.getElementById("warn-tel-name").innerHTML = "Please enter username.";
                return false;
            } else {
                document.getElementById("warn-tel-name").innerHTML = "";
            }
            var paternmb = /^[0]{1}[0-9]{8,11}$/;
            // CHeck nhập tel-otp
            var telotp = $('#tel-number-otp').val();
            if (telotp.length <= 0) {
                document.getElementById("warn-tel-otp").innerHTML = "Please enter mobile number received otp.";
                return false;
            } else {
                document.getElementById("warn-tel-otp").innerHTML = "";
            };
            var check2 = paternmb.test(telotp);
            if (!check2) {
                document.getElementById("warn-tel-otp").innerHTML = "Invalid Mobile number in Mobile number receive OTP. Please check!";
                return false;
            }
            else {
                document.getElementById("warn-tel-otp").innerHTML = "";
            }
        var confirmMesg = "Do you want to send for approval?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var model = {
                CifNo: document.getElementById('cus-cif').innerHTML,
                IdNumber: document.getElementById('cus-idpassport').innerHTML,
                CusName: document.getElementById('cus-name').innerHTML,
                CifBranch: $('#branch-cif').val(),
                CusType: $('#cus-type').val(),
                BirthDay: $('#cus-birth').val(),
                Residence: $('#cus-resident').val(),
                Email: $('#cus-email').val(),
                Address: $('#cus-address').val(),
                Telephone: $('#tel-name').val(),
                AccountNo: acountDefault,
                TelephoneOtp: $('#tel-number-otp').val(),
                Gender: $('#cus-sex').val(),
                Contact: $('#contact').val(),
                StaffCode: $('#cus-staff-code').val(),
                CreatedUser: $('#user-created').val(),
                BranchCodeCreatedUser: $('#branch-code').val(),
                PosCodeCreatedUser: $('#pos-code').val(),
                PackageCode: $('#pkg-code').val(),
                ListAcount: liAcount,
            };
            $http.post(getRootUrl() + '/Customer/CreateCustomer', model)
            .then(function (data) {
                if (redirecToLoginByTypeOf(data.data)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {

                    //alert(data.data.data);
                    if (data.data.id !== 0) {
                        document.getElementById('notify-result').innerHTML = data.data.data;
                        document.getElementById('notify-result').style.color = "blue";
                        idCustomer = data.data.id;
                        if (!$('#save-customer').hasClass('hidden')) {
                            $('#save-customer').addClass('hidden');
                            $('#approval-customer').removeClass('hidden');
                        }
                    } else {
                        $('#notify-result').addClass('red');
                        document.getElementById('notify-result').innerHTML = data.data.data;
                        document.getElementById('notify-result').style.color = "red";
                    }
                }
            });
            return false;
        }
    };
    function searchCustomer() {
        document.getElementById('notify-customer').innerHTML = "";
        //hide info
        if ($('#info').hasClass('show')) {
            $('#info').removeClass('show').addClass('hidden');
        }
        $('#progess').modal("show");
        $http({
            method: "post",
            url: getRootUrl() + "/Customer/SearchCustomer",
            params: {
                cif: JSON.stringify($('#cif').val()),
                id: JSON.stringify($('#id-passport').val())
            }
        }).then(function (msg) {
            $('#progess').modal("hide");           
            if (msg.data === "1") {
                document.getElementById('notify-customer').innerHTML = "This customer already was registered in the system.";
                document.getElementById('notify-customer').style.color = "red";
                return false;
            }
            if (msg.data === "2") {
                document.getElementById('notify-customer').innerHTML = "The system is in maintenance mode, please re-try later..";
                document.getElementById('notify-customer').style.color = "red";
                return false;
            }
            else if (msg.data === "0") {
                document.getElementById('notify-customer').innerHTML = "Customer information does not exist in CoreBank system.";
                document.getElementById('notify-customer').style.color = "red";
                return false;
            }
            else if (redirecToLoginByTypeOf(msg.data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            }
            else {
                liAcount = msg.data.ListAcount;
                var model = msg.data;
                $scope.datas = model.ListAcount;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);

                if (model.Count > 1) {
                    document.getElementById('notify-customer').innerHTML = "Có tổng số " + model.Count + " kết quả trả về. Giao dịch viên chú ý kiểm tra thông tin và tìm kiếm theo số cif từ SIBS để đảm bảo chính xác thông tin khách hàng đăng ký.";
                }
                document.getElementById('cus-cif').innerHTML = model.CifNo;
                document.getElementById('cus-idpassport').innerHTML = model.IdNumber;
                document.getElementById('cus-name').innerHTML = model.CusName;
                if (model.gender !== "" && model.gender !== null) {
                    $('#cus-sex').val(model.gender);
                }
                if (model.CifBranch !== "" && model.CifBranch !== null) {
                    $('#branch-cif').val(model.CifBranch);
                }
                if (model.Residence !== "" && model.Residence !== null) {
                    $('#cus-resident').val(model.Residence);
                }
                $('#cus-sex').val(model.Gender);
                $('#pos-code').val(model.PosCodeCreatedUser);
                $('#user-created').val(model.CreatedUser);
                $('#cus-birth').val(model.BirthDay);
                $('#cus-email').val(model.Email);
                $('#cus-address').val(model.Address);
                $('#branch-code').val(model.BranchCodeCreatedUser);
                $('#cus-type').val(model.CusType);
                $('#contact').val(model.Contact);
                if ($('#info').hasClass('hidden')) {
                    $('#info').removeClass('hidden').addClass('show');
                }
                if ($('#save-customer').hasClass('hidden')) {
                    $('#save-customer').removeClass('hidden');
                }
                if (!$('#approval-customer').hasClass('hidden')) {
                    $('#approval-customer').addClass('hidden');
                }
            }
        });
    }
});
systemSearchApp.controller('confirmCusController', function ($scope, $http, $timeout) {
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';
    // At least Safari 3+: "[object HTMLElementConstructor]"
    var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    // Internet Explorer 6-11
    var isIE = false || !!document.documentMode;
    // Edge 20+
    var isEdge = !isIE && !!window.StyleMedia;
    // Chrome 1+
    var isChrome = !!window.chrome && !!window.chrome.webstore;
    // Blink engine detection
    var isBlink = (isChrome || isOpera) && !!window.CSS;

    if (isEdge === true || isIE === true) {
        if (document.URL.indexOf("#") <= 0) {
            url = document.URL + "#";
            location = "#";
            //Reload the page
            location.reload(true);
        } else {
            getCustomerConfirmListSearch();
        }
    } else {
        getCustomerConfirmListSearch();
    }

    //getCustomerConfirmListSearch();
    $scope.customerSearch = function () {
        getCustomerConfirmListSearch();
    };
    $scope.customerApprove = function () {
        var models = [];
        angular.forEach($scope.filtered, function (item) {
            if (item.Selected === true) {
                models.push(item);
            }
        });
        if (models.length <= 0) {
            alert("Please select the records to send approve!");
            return false;
        }
        var confirmMesg = "Do you want to sending approval this customers?";
        var r = confirm(confirmMesg);
        if (r === true) {
            $http.post(getRootUrl() + '/Customer/ApprovalCustomer', models)
                .then(function (data) {
                    if (redirectToLogin(data)) {
                        window.location.href = getRootUrl() + "/Error/Index";
                    } else {
                        alert(data.data.toString());
                        if (isEdge === true || isIE === true || isFirefox === true) {
                            return window.location.reload(true);
                        } else {
                            getCustomerConfirmListSearch();
                            $scope.selectedAll = false;
                        }

                    }
                });
        }

    };
    $scope.customerReject = function () {
        var models = [];
        angular.forEach($scope.filtered, function (item) {
            if (item.Selected === true) {
                models.push(item);
            }
        });
        if (models.length <= 0) {
            alert("Please select the records to reject!");
            return false;
        }
        var confirmMesg = "Do you want to reject this customer?";
        var r = confirm(confirmMesg);
        if (r === true) {
            $http.post(getRootUrl() + '/Customer/RejectCustomer', models)
                .then(function (data) {
                    if (redirectToLogin(data)) {
                        window.location.href = getRootUrl() + "/Error/Index";
                    } else {
                        alert(data.data.toString());
                        if (isEdge === true || isIE === true || isFirefox === true) {
                            return window.location.reload(true);
                        } else {
                            getCustomerConfirmListSearch();
                            $scope.selectedAll = false;
                        }

                    }
                });
        }
    };
    $scope.checkAll = function () {
        if ($scope.selectedAll) {
            $scope.selectedAll = true;
        } else {
            $scope.selectedAll = false;
        }
        angular.forEach($scope.filtered, function (item) {
            item.Selected = $scope.selectedAll;
        });
    };
    $(document).keypress(function (e) {
        if (e.which === 13 && (e.target.id === "user-name" || e.target.id === "status" || e.target.id === "id-passport" ||
        e.target.id === "cif" || e.target.id === "cus-name" || e.target.id === "fromDate" || e.target.id === "toDate")) {
            getCustomerConfirmListSearch();
        }
    });

    function getCustomerConfirmListSearch() {
        var model = {
            SentUser: $("#user-name").val(),
            Status: $("#status").val(),
            IdPassport: $("#id-passport").val(),
            Cif: $("#cif").val(),
            CusName: $('#cus-name').val(),
            FromDate: $("#fromDate").val(),
            ToDate: $("#toDate").val(),
            Branch: $("#branch-cif").val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Customer/GetListCutomerConfirm",
            params: {
                cusName: JSON.stringify(model.CusName),
                user: JSON.stringify(model.SentUser),
                listStatus: JSON.stringify(model.Status),
                idPassport: JSON.stringify(model.IdPassport),
                cif: JSON.stringify(model.Cif),
                fd: JSON.stringify(model.FromDate),
                td: JSON.stringify(model.ToDate),
                branch: JSON.stringify(model.Branch)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller('updateCusController', function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getLisCusUpdate();
    $scope.cusUpdateSearch = function () {
        getLisCusUpdate();
    };
    $scope.pageChanged = function () {
        getLisCusUpdate();
    };
    $scope.cancelregistcus = function (item) {
        var r = confirm("This customer will be deleted out of the Mobile banking system. Are you sure to delete?");
        if (r === true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Customer/CancelRegister",
                params: {
                    id: JSON.stringify(item.Id),
                    status: JSON.stringify("A")
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    window.location.reload();
                }
            });
        } else {
            return false;
        }

    };
    $(document).keypress(function (e) {
        if (e.which === 13 && (e.target.id === "account-no" || e.target.id === "id-passport" ||
        e.target.id === "cif" || e.target.id === "user-name")) {
            getLisCusUpdate();
        }
    });
    function getLisCusUpdate() {
        var model = {
            Cif: $('#cif').val(),
            AccountNo: $('#account-no').val(),
            CustomerName: $("#cus-name").val(),
            UserName: $('#user-name').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Customer/GetListUpdateCustomers",
            params: {
                cif: JSON.stringify(model.Cif),
                accountNo: JSON.stringify(model.AccountNo),
                cusName: JSON.stringify(model.CustomerName),
                userName: JSON.stringify(model.UserName),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});
systemSearchApp.controller('editCusController', function ($scope, $http) {
    $scope.sendapproval = function () {
        if ($('#cus-birth').val() === "") {
            document.getElementById("warn-birthday").innerHTML = "Please enter Birthday!";
            return false;
        } else {
            document.getElementById("warn-birthday").innerHTML = "";
        }
        //check Email đúng định dạng
        if ($('#cus-email').val() != "") {
            var pw = $('#cus-email').val();
            var pattern = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
            var check = pattern.test(pw);
            if (!check) {
                document.getElementById('warn-email').innerHTML = "Invalid email. Please check.";
                return false;
            } else {
                document.getElementById('warn-email').innerHTML = "";
            }
        }
        //Check chọn tài khoản default
        var demcheck = 0;
        var acountDefault;
        var accBranch;
        var accCurr;
        var accType;
        $("input[type=radio]").each(function () {
            var id1 = $(this).attr("id");
            if (document.getElementById(id1).checked === true) {
                demcheck++;
                acountDefault = id1;
            }
        });
        // Lấy thông tin update Acount
        $("td").each(function () {
            var classAcc = $(this).attr("class");
            if (classAcc === acountDefault + "-acctype") {
                accType = document.getElementsByClassName(classAcc)[0].innerText;
            }
            if (classAcc === acountDefault + "-ccy") {
                accCurr = document.getElementsByClassName(classAcc)[0].innerText;
            }
            if (classAcc === acountDefault + "-branchCode") {
                accBranch = document.getElementsByClassName(classAcc)[0].innerText;
            }
        });

        if (demcheck === 0) {
            alert("Please choose Default account!");
            return false;
        }
        
        //Check nhập username
        var telname = $('#tel-name').val();
        if (telname === "") {
            document.getElementById("warn-tel-name").innerHTML = "Please enter username.";
            return false;
        } else {
            document.getElementById("warn-tel-name").innerHTML = "";
        }
        // CHeck nhập tel-otp
        var telotp = $('#tel-number-otp').val();
        if (telotp === "") {
            document.getElementById("warn-tel-otp").innerHTML = "Please enter mobile number recived otp.";
            return false;
        } else {
            document.getElementById("warn-tel-otp").innerHTML = "";
        }
        var model = {
            Id: $('#cus-id').val(),
            Birthday: $('#cus-birth').val(),
            Email: $('#cus-email').val(),
            Address: $('#cus-address').val(),
            CusStatus: $('#cus-status').val(),
            Gender: $('#cus-sex').val(),
            CusType: $('#cus-type').val(),
            Contact: $('#contact').val(),
            StaffCode: $('#cus-staff-code').val(),
            //Segmentation: $('#cus-segment').val(),
            Resident: $('#cus-resident').val(),
            AccountNo: acountDefault,
            AccountType: accType.replace(" ", ""),
            AccountCurr: accCurr.replace(" ", ""),
            AcountBranch: accBranch.replace(" ", ""),
            MobileNo: $('#tel-number').val(),
            MobileNumberOTP: $('#tel-number-otp').val(),
            Package: $('#pkg-code').val(),
        };       
        $http.post(getRootUrl() + '/Customer/UpdateCustomer', model)
            .then(function (data) {
                if (redirectToLogin(data)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(data.data.msg.toString());
                    //if (data.data.toString() === "Mã cán bộ không hợp lệ.")
                    //    return false;
                    if (data.data.id === "1") {
                        window.location.href = getRootUrl() + "/Customer/Index?f=4&c=100";
                    }
                }
            });
    };
    $scope.cancelregistcus = function () {
        var r = confirm("Do you want delete this customer?");
        if (r === true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Customer/CancelRegister",
                params: {
                    id: $('#cus-id').val(),
                    status: $('#cus-status').val()
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    window.location.href = getRootUrl() + "/Customer/Index?f=4&c=100";
                }
            });
        } else {
            return false;
        }

    };

});
systemSearchApp.controller('managerCusController', function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';
    // At least Safari 3+: "[object HTMLElementConstructor]"
    var isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    // Internet Explorer 6-11
    var isIE = false || !!document.documentMode;
    // Edge 20+
    var isEdge = !isIE && !!window.StyleMedia;
    // Chrome 1+
    var isChrome = !!window.chrome && !!window.chrome.webstore;
    // Blink engine detection
    var isBlink = (isChrome || isOpera) && !!window.CSS;

    if (isEdge === true || isIE === true || isFirefox === true) {
        if (document.URL.indexOf("#") <= 0) {
            url = document.URL + "#";
            location = "#";
            //Reload the page
            window.location.reload(true);
        } else {
            getCusManagerListSearch();
        }
    } else {
        getCusManagerListSearch();
    }

    $scope.cusManagerSearch = function () {
        getCusManagerListSearch();
    };
    $scope.pageChanged = function () {
        getCusManagerListSearch();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn muốn thay đổi trạng thái của bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Customer/ChangeStatusCustomer",
                params: {
                    id: JSON.stringify(item.MobileId)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    if (isEdge === true || isIE === true || isFirefox === true) {
                        return window.location.reload(true);
                    } else {
                        getCusManagerListSearch();
                    }
                }
            });
        } else {
            return false;
        }
    };
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "name" || e.target.id === "status" || e.target.id === "code" )) {
            getCusManagerListSearch();
        }
    });
    function getCusManagerListSearch() {
        var model = {
            Name: $('#name').val(),
            Code: $("#code").val(),
            Status: $("#status").val(),
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Customer/GetListCustomer",
            params: {
                code: JSON.stringify(model.Code),
                name: JSON.stringify(model.Name),
                status: JSON.stringify(model.Status)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("templateController", function ($scope, $http, $timeout) {
    getList();
    $scope.templateSearch = function () {
        getList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Bạn có chắc chắn muốn thay đổi trạng thái bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Template/ChangeStatusTemplate",
                params: {
                    code: JSON.stringify(item.TemplateCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getList();
                }
            });
        } else {
            return false;
        }
    };
    $scope.configReset = function (item) {
        var confirmMesg = "Bạn có muốn xóa bộ đệm?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Template/ResetCache",
                params: {

                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListConfig();
                }
            });
        } else {
            return false;
        }
    };
    function getList() {
        var model = {
            TemplateCode: $('#tem-code').val(),
            TemplateName: $('#tem-name').val()
            
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Template/GetListTemplate",
            params: {
                code: JSON.stringify(model.TemplateCode),
                name: JSON.stringify(model.TemplateName)
                //name_en: JSON.stringify(model.TemplateNameEN),
                //status: JSON.stringify(model.TemplateStatus)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("customerTypeController", function ($scope, $http, $timeout) {
    getListConfig();

    $scope.custypeSearch = function () {
        getListConfig();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Do you want to Thay đổi trạng thái of this Customer Type?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/CustomerType/ChangeStatusCusType",
                params: {
                    code: JSON.stringify(item.CusTypeCode)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getListConfig();
                }
            });
        } else {
            return false;
        }
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && e.target.id === "cus-type") {
            getListConfig();
        }
    });

    function getListConfig() {
        var model = {
            CusTypeCode: $('#custype-code').val(),
            CusTypeName: $('#custype-name').val(),
            CusTypeStatus: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/CustomerType/GetListCustype",
            params: {
                code: JSON.stringify(model.CusTypeCode),
                name: JSON.stringify(model.CusTypeName),
                status: JSON.stringify(model.CusTypeStatus),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10;
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("promotionController", function ($scope, $http, $timeout) {
    getList();

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "code" || e.target.id === "name" || e.target.id === "status")) {
            getList();
        }
    });
    $scope.promotionSearch = function () {
        getList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Do you want to Thay đổi trạng thái of this Promotion?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Promotion/ChangeStatusPromotion",
                params: {
                    code: JSON.stringify(item.PromotionCode),
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getList();
                }
            });
        } else {
            return false;
        }
    };
    function getList() {
        var model = {
            PromotionCode: $('#code').val(),
            PromotionName: $('#name').val(),
            PromotionStatus: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Promotion/GetListPromotion",
            params: {
                code: JSON.stringify(model.PromotionCode),
                name: JSON.stringify(model.PromotionName),
                status: JSON.stringify(model.PromotionStatus)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("bannerController", function ($scope, $http, $timeout) {
    getList();

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "url" || e.target.id === "iamge" || e.target.id === "status")) {
            getList();
        }
    });
    $scope.bannerSearch = function () {
        getList();
    };
    $scope.changeStatuslockHome = function (item) {
        var confirmMesg = "Bạn có chắc chắn khóa bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Banner/ChangeStatusBannerHome",
                params: {
                    code: JSON.stringify(item.Code),
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getList();
                }
            });
        } else {
            return false;
        }
    };
    $scope.changeStatuslock = function (item) {
        var confirmMesg = "Bạn có chắc chắn khóa bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Banner/ChangeStatusBanner",
                params: {
                    code: JSON.stringify(item.Code),
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getList();
                }
            });
        } else {
            return false;
        }
    };
    $scope.changeStatusUnlock = function (item) {
        var confirmMesg = "Bạn có chắc chắn muốn mở khóa bản ghi này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/Banner/ChangeStatusBanner",
                params: {
                    code: JSON.stringify(item.Code),
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getList();
                }
            });
        } else {
            return false;
        }
    };
    function getList() {
        var model = {
            type: $('#type').val(),
            name: $('#name').val(),
            status: $('#status').val()
        };
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/Banner/GetListBanner",
            params: {
                type: JSON.stringify(model.type),
                name: JSON.stringify(model.name),
                status: JSON.stringify(model.status)
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});
systemSearchApp.controller("bannerSortController", function ($scope, $http, $timeout) {
    getList();

    //$(document).keypress(function (e) {
    //    if (e.which == 13 && (e.target.id === "url" || e.target.id === "iamge" || e.target.id === "status")) {
    //        getList();
    //    }
    //});
    //$scope.promotionSearch = function () {
    //    getList();
    //};
    $scope.clearOnFocus = function (item) {
        if (item.Order == "0") {
            item.Order = "";
        }
    }
    $scope.sortBanner = function (datas) {
        console.log(datas);
        var check = true;
        datas.forEach(element => {
            if (element.Order == "") {
                alert("Vui lòng nhập giá trị cho Banner: " + element.Name);
        check = false;
    }
            
    return false;
});

if (check) {
    var model = {
        listBannerSort: JSON.stringify(datas)
    };
    var response = $http({
        //contentType: 'application/json; charset=utf-8',
        method: "post",
        url: getRootUrl() + "/Banner/UpdateBannerSort",
        params: {
            listBannerSort: model.listBannerSort,
        }
    });
    response.then(function (msg) {
        if (redirectToLogin(msg)) {
            window.location.href = getRootUrl() + "/Error/Index";
        } else {
            alert(msg.data.toString());
            getList();
        }
    });
}
        
//var confirmMesg = "Do you want to Thay đổi trạng thái of this Banner?";
//var r = confirm(confirmMesg);
//if (r == true) {

//} else {
//    return false;
//}
};
function getList() {

    var datas = $http({
        method: "get",
        url: getRootUrl() + "/Banner/GetListBannerSort",
        params: {

        }
    });
    datas.success(function (data) {
        if (redirecToLoginByTypeOf(data)) {
            window.location.href = getRootUrl() + "/Error/Index";
        } else {
            $scope.datas = data;
            $scope.filteredItems = $scope.datas.length;
            $scope.maxSize = 5;
            $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
            //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
            $scope.currentPage = 1;
            $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
        }
    });
    $scope.filter = function () {
        $timeout(function () {
            $scope.filteredItems = $scope.filtered.length;
            $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
        }, 10);
    };
}
});
systemSearchApp.controller("userSessionController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.userSessionSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };
    $(document).keypress(function (e) {
        if (e.which == 13 && e.target.id === "user-id") {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/UserCurrentSession/GetList",
            params: {
                userName: JSON.stringify($('#user-name').val()),
                branch: JSON.stringify($('#branch').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});

systemSearchApp.controller("userActionHisController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.userActionHisSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };

    $scope.viewDetail = function (item) {
        $http({
            method: "post",
            url: getRootUrl() + "/UserActionHistory/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg) {

            if (redirectToLogin(msg)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                var model = msg.data;
                document.getElementById('info-username').innerHTML = model.UserName;
                document.getElementById('info-fullname').innerHTML = model.FullName;
                //document.getElementById('info-city').innerHTML = model.City;
                document.getElementById('info-branch').innerHTML = model.Branch;
                //document.getElementById('info-pos').innerHTML = model.Pos;
                document.getElementById('info-func').innerHTML = model.Function;
                document.getElementById('info-action').innerHTML = model.Action;
                document.getElementById('info-actiontime').innerHTML = model.ActionTime;
                document.getElementById('info-actiondesc').innerHTML = model.ActionDesc;
                $('#viewDetail').modal("show");
            }
        });
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "actionType" || e.target.id === "user-name" ||
           e.target.id === "fromDate" || e.target.id === "toDate")) {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/UserActionHistory/GetList",
            params: {
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                userName: JSON.stringify($('#user-name').val()),
                branch: JSON.stringify($('#branch').val()),
                actionType: JSON.stringify($('#actionType').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});

systemSearchApp.controller("userSessionHisController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.userSessionHisSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "user-id" || e.target.id === "fromDate" || e.target.id === "toDate")) {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/UserSessionHistory/GetList",
            params: {
                userName: JSON.stringify($('#user-name').val()),
                branch: JSON.stringify($('#branch').val()),
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });

        $scope.viewDetail = function (item) {
            $http({
                method: "post",
                url: getRootUrl() + "/UserSessionHistory/ViewDetail",
                params: {

                    id: JSON.stringify(item.Id),
                }
            }).then(function (msg) {

                if (redirectToLogin(msg)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    var model = msg.data;
                    document.getElementById('info-mobile').innerHTML = model.MobileNo;
                    document.getElementById('info-request').innerHTML = model.Request;
                    document.getElementById('info-device').innerHTML = model.Device;
                    document.getElementById('info-os').innerHTML = model.OS;
                    document.getElementById('info-status').innerHTML = model.Status;
                    document.getElementById('info-requestdate').innerHTML = model.RequestDate;
                    document.getElementById('info-respondseddate').innerHTML = model.RespondsedDate;
                    document.getElementById('info-desc').innerHTML = model.Description;
                    document.getElementById('info-rescode').innerHTML = model.ResCode;
                    document.getElementById('info-msgcode').innerHTML = model.MsgCode;
                    document.getElementById('info-deviceid').innerHTML = model.DeviceID;
                    $('#viewDetail').modal("show");
                }
            });
        };
    }
});
systemSearchApp.controller("hisSmsController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.hisSmsSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };
    $scope.viewDetail = function (item) {
        $http({
            method: "post",
            url: getRootUrl() + "/HistorySMS/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg) {

            if (redirectToLogin(msg)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                var model = msg.data;
                document.getElementById('info-mobile').innerHTML = model.MobileNo;
                document.getElementById('info-branch').innerHTML = model.BranchName;
                document.getElementById('info-keyword').innerHTML = model.Keyword;
                document.getElementById('info-kind').innerHTML = model.SmsKind;
                document.getElementById('info-sendfr').innerHTML = model.SendFrom;
                document.getElementById('info-result').innerHTML = model.Result;
                document.getElementById('info-sentdate').innerHTML = model.SentDate;
                document.getElementById('info-recdate').innerHTML = model.ReceivedDate;
                document.getElementById('info-content').innerHTML = model.Content;
                //document.getElementById('info-mobile').innerHTML = model.MobileNo;
                $('#viewDetail').modal("show");
            }
        });
    };
    $scope.resend = function (item) {
        $http({
            method: "post",
            url: getRootUrl() + "/HistorySMS/Resend",
            params: {
                id: JSON.stringify(item.Id),
                mobile: JSON.stringify(item.MobileNo),
                keyword: JSON.stringify(item.Keyword),
                content: JSON.stringify(item.Content),
            }
        }).then(function (data) {

            if (redirectToLogin(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                alert(data.data);
                window.location.reload();
            }
        });
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "branch" || e.target.id === "mobile-no") || e.target.id === "sendfr"
            || e.target.id === "kind" || e.target.id === "keyword" || e.target.id === "result" || e.target.id === "fromDate" || e.target.id === "toDate") {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/HistorySMS/GetList",
            params: {
                branchCode: JSON.stringify($('#branch').val()),
                mobile: JSON.stringify($('#mobile-no').val()),
                keyword: JSON.stringify($('#keyword').val()),
                kind: JSON.stringify($('#kind').val()),
                sendfr: JSON.stringify($('#sendfr').val()),
                result: JSON.stringify($('#result').val()),
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});
systemSearchApp.controller("requestLogController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.requestSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };
    $scope.changeStatus = function (item) {
        var confirmMesg = "Do you want to Thay đổi trạng thái of this record?";
        var r = confirm(confirmMesg);
        if (r == true) {
            var response = $http({
                method: "post",
                url: getRootUrl() + "/RequestLog/ChangeStatus",
                params: {
                    code: JSON.stringify(item.Id),
                    status: JSON.stringify(item.Status)
                }
            });
            response.then(function (msg) {
                if (redirectToLogin(msg)) {
                    window.location.href = "/Error/Index";
                } else {
                    alert(msg.data.toString());
                    getList();
                }
            });
        } else {
            return false;
        }
    };
    $(document).keypress(function (e) {
        if ( e.target.id === "status" || e.target.id === "fromDate" || e.target.id === "toDate") {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/RequestLog/GetList",
            params: {
                status: JSON.stringify($('#status').val()),
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
        $scope.filter = function () {
            $timeout(function () {
                $scope.filteredItems = $scope.filtered.length;
                $scope.numberPages = Math.ceil($scope.filtered.length / $scope.PerPageItems);
            }, 10);
        };
    }
});

systemSearchApp.controller("ussdController", function ($scope, $http) {
    
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();

    $scope.UssdSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "mobile-no") || e.target.id === "requesttype"
            || e.target.id === "status" || e.target.id === "fromDate" || e.target.id === "toDate") {
            getList();
        }
    });

    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/UssdLog/GetListUssdLog",
            params: {
                mobileNo: JSON.stringify($('#mobile-no').val()),
                requestType: JSON.stringify($('#requesttype').val()),
                status: JSON.stringify($('#status').val()),
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});

systemSearchApp.controller("productRegistController", function ($scope, $http)
{
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.productRegistSearch = function ()
    {
        getList();
    };
    $scope.pageChanged = function ()
    {
        getList();
    };
    $scope.productRegistApprove = function ()
    {
        var models = [];
        angular.forEach($scope.datas, function (item)
        {
            if (item.Selected == true)
            {
                models.push(item);
            }
        });
        if (models.length <= 0)
        {
            alert("Bạn phải lựa chọn ít nhất 1 bản ghi để duyệt, và chỉ có thể thực hiện duyệt đối với các bản ghi có trạng thái Inital.");
            return false;
        }
        $http.post(getRootUrl() + '/ProductRegistration/ApprovalProductRegist', models)
            .then(function (data)
            {
                if (redirectToLogin(data))
                {
                    window.location.href=  getRootUrl() + "/Error/Index";
                } else
                {
                    getList();
                    $scope.selectedAll = false;
                }
            });


    };
    $scope.productRegistReject = function ()
    {
        var models = [];
        angular.forEach($scope.datas, function (item)
        {
            if (item.Selected == true)
            {
                models.push(item);
            }
        });
        if (models.length <= 0)
        {
            alert("Bạn phải lựa chọn ít nhất 1 bản ghi để từ chối duyệt, và chỉ có thể thực hiện duyệt đối với các bản ghi có trạng thái Inital.");
            return false;
        }
        $http.post('/ProductRegistration/RejectProductRegist', models)
            .then(function (data)
            {
                if (redirectToLogin(data))
                {
                    window.location.href=  getRootUrl() + "/Error/Index";
                } else
                {
                    getList();
                    $scope.selectedAll = false;
                }
            });

    };
    $scope.checkAll = function ()
    {
        if ($scope.selectedAll)
        {
            $scope.selectedAll = true;
        } else
        {
            $scope.selectedAll = false;
        }
        angular.forEach($scope.datas, function (item)
        {
            if (item.StatusId == '0')
            {
                item.Selected = $scope.selectedAll;
            }
        });
    };
    $scope.sendApproval = function (item)
    {
        $http({
            method: "post",
            url: "/ProductRegistration/SendApproval",
            params: {
                id: JSON.stringify(item.Id)
            }
        }).then(function (msg)
        {

            if (redirectToLogin(msg))
            {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else
            {
                getList();
            }
        });
    };
    $scope.viewDetail = function (item)
    {
        var models = [];
        if (!$('#confirm').hasClass('hidden')) {
            $('#confirm').addClass('hidden');
        }
        if (item.StatusId == "0") {
            $('#confirm').removeClass('hidden');
        }
        $http({
            method: "post",
            url: "/ProductRegistration/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg)
        {

            if (redirectToLogin(msg))
            {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else
            {
                var model = msg.data;
                document.getElementById('info-mobile').innerHTML = model.Mobile;
                document.getElementById('info-branch').innerHTML = model.BranchName;
                document.getElementById('info-idnumber').innerHTML = model.IdNumber;
                document.getElementById('info-cusname').innerHTML = model.CusName;
                document.getElementById('info-status').innerHTML = model.StatusName;
                document.getElementById('info-sendapprovaldate').innerHTML = model.SendApprovalDate;
                document.getElementById('info-product').innerHTML = model.ProductName;
                document.getElementById('info-createddate').innerHTML = model.CreatedDate;
                document.getElementById('info-sendapprovaluser').innerHTML = model.SendApprovalUser;
                document.getElementById('info-confirmdate').innerHTML = model.ConfirmDate;
                document.getElementById('info-confirmuser').innerHTML = model.ConfirmUser;
                $('#viewDetail').modal("show");
            }
        });
        $('#approval').click(function() {
            models.push(item);
            if (models.length <= 0) {
                alert("Bạn phải lựa chọn ít nhất 1 bản ghi để duyệt, và chỉ có thể thực hiện duyệt đối với các bản ghi có trạng thái Inital.");
                return false;
            }
           $http.post('/ProductRegistration/ApprovalProductRegist', models)
          .then(function (data)
          {
              if (redirectToLogin(data))
              {
                  window.location.href=  getRootUrl() + "/Error/Index";
              } else
              {
                  getList();
              }
          });
        });
        $('#reject').click(function() {
            models.push(item);
            if (models.length <= 0)
            {
                alert("Bạn phải lựa chọn ít nhất 1 bản ghi để duyệt, và chỉ có thể thực hiện duyệt đối với các bản ghi có trạng thái Inital.");
                return false;
            }
            $http.post('/ProductRegistration/RejectProductRegist', models)
           .then(function (data)
           {
               if (redirectToLogin(data))
               {
                   window.location.href=  getRootUrl() + "/Error/Index";
               } else
               {
                   getList();
               }
           });
        });
    };

    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "id-number" || e.target.id === "cus-name") || e.target.id === "mobile-number"
            || e.target.id === "branch" || e.target.id === "fromDate" || e.target.id === "toDate" || e.target.id === "status")
        {
            getList();
        }
    });
    function getList()
    {
        var datas = $http({
            method: "get",
            url: "/ProductRegistration/GetList",
            params: {
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                mobile: JSON.stringify($('#mobile-number').val()),
                idnumber: JSON.stringify($('#id-number').val()),
                cusname: JSON.stringify($('#cus-name').val()),
                status: JSON.stringify($('#status').val()),
                branch: JSON.stringify($('#branch').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});

systemSearchApp.controller("confirmProductRegistController", function ($scope, $http)
{
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.confirmProductRegistSearch = function ()
    {
        getList();
    };
    $scope.pageChanged = function ()
    {
        getList();
    };
    $scope.confirmProductRegistApprove = function ()
    {
        var models = [];
        angular.forEach($scope.datas, function (item)
        {
            if (item.Selected == true)
            {
                models.push(item);
            }
        });
        if (models.length <= 0)
        {
            alert("Bạn phải lựa chọn ít nhất 1 bản ghi để duyệt.");
            return false;
        }
        $http.post('/ConfirmProductRegist/ApprovalProductRegist', models)
            .then(function (data)
            {
                if (redirectToLogin(data))
                {
                    window.location.href=  getRootUrl() + "/Error/Index";
                } else
                {
                    getList();
                    $scope.selectedAll = false;
                }
            });


    };
    $scope.confirmProductRegistReject = function ()
    {
        var models = [];
        angular.forEach($scope.datas, function (item)
        {
            if (item.Selected == true)
            {
                models.push(item);
            }
        });
        if (models.length <= 0)
        {
            alert("Bạn phải lựa chọn ít nhất 1 bản ghi để từ chối duyệt.");
            return false;
        }
        $http.post('/ConfirmProductRegist/RejectProductRegist', models)
            .then(function (data)
            {
                if (redirectToLogin(data))
                {
                    window.location.href=  getRootUrl() + "/Error/Index";
                } else
                {
                    getList();
                    $scope.selectedAll = false;
                }
            });

    };
    $scope.checkAll = function ()
    {
        if ($scope.selectedAll)
        {
            $scope.selectedAll = true;
        } else
        {
            $scope.selectedAll = false;
        }
        angular.forEach($scope.datas, function (item)
        {
            item.Selected = $scope.selectedAll;
        });
    };
    $scope.viewDetail = function (item)
    {
        var models = [];
        $http({
            method: "post",
            url: "/ConfirmProductRegist/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg)
        {

            if (redirectToLogin(msg))
            {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else
            {
                var model = msg.data;
                document.getElementById('info-mobile').innerHTML = model.Mobile;
                document.getElementById('info-branch').innerHTML = model.BranchName;
                document.getElementById('info-idnumber').innerHTML = model.IdNumber;
                document.getElementById('info-cusname').innerHTML = model.CusName;
                document.getElementById('info-status').innerHTML = model.StatusName;
                document.getElementById('info-sendapprovaldate').innerHTML = model.SendApprovalDate;
                document.getElementById('info-product').innerHTML = model.ProductName;
                document.getElementById('info-createddate').innerHTML = model.CreatedDate;
                document.getElementById('info-sendapprovaluser').innerHTML = model.SendApprovalUser;
                document.getElementById('info-confirmdate').innerHTML = model.ConfirmDate;
                document.getElementById('info-confirmuser').innerHTML = model.ConfirmUser;
                $('#viewDetail').modal("show");
            }
        });
        $('#approval').click(function ()
        {
            models.push(item);
            if (models.length <= 0)
            {
                alert("Bạn phải lựa chọn ít nhất 1 bản ghi để duyệt, và chỉ có thể thực hiện duyệt đối với các bản ghi có trạng thái Inital.");
                return false;
            }
            $http.post('/ConfirmProductRegist/ApprovalProductRegist', models)
           .then(function (data)
           {
               if (redirectToLogin(data))
               {
                   window.location.href=  getRootUrl() + "/Error/Index";
               } else
               {
                   getList();
               }
           });
        });
        $('#reject').click(function ()
        {
            models.push(item);
            if (models.length <= 0)
            {
                alert("Bạn phải lựa chọn ít nhất 1 bản ghi để duyệt, và chỉ có thể thực hiện duyệt đối với các bản ghi có trạng thái Inital.");
                return false;
            }
            $http.post('/ConfirmProductRegist/RejectProductRegist', models)
           .then(function (data)
           {
               if (redirectToLogin(data))
               {
                   window.location.href=  getRootUrl() + "/Error/Index";
               } else
               {
                   getList();
               }
           });
        });
    };

    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "id-number" || e.target.id === "cus-name") || e.target.id === "mobile-number"
            || e.target.id === "branch" || e.target.id === "fromDate" || e.target.id === "toDate" || e.target.id === "status")
        {
            getList();
        }
    });
    function getList()
    {
        var datas = $http({
            method: "get",
            url: "/ConfirmProductRegist/GetList",
            params: {
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                mobile: JSON.stringify($('#mobile-number').val()),
                idnumber: JSON.stringify($('#id-number').val()),
                cusname: JSON.stringify($('#cus-name').val()),
                status: JSON.stringify($('#status').val()),
                branch: JSON.stringify($('#branch').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href=  getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});

systemSearchApp.controller("hisTransController", function ($scope, $http)
{
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.hisTransSearch = function ()
    {
        getList();
    };
    $scope.pageChanged = function ()
    {
        getList();
    };
   
    $scope.viewDetail = function (item)
    {
        $http({
            method: "post",
            url: getRootUrl() + "/HistoryTransaction/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg)
        {

            if (redirectToLogin(msg))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                var model = msg.data;
                document.getElementById('info-id').innerHTML = model.Id;
                document.getElementById('info-mobile').innerHTML = model.Mobile;
                document.getElementById('info-fracc').innerHTML = model.FromAccount;
                document.getElementById('info-toacc').innerHTML = model.ToAccount;
                document.getElementById('info-service').innerHTML = model.Service;
                document.getElementById('info-service-type').innerHTML = model.ServiceType;
                document.getElementById('info-transtime').innerHTML = model.TransTime;
                document.getElementById('info-transnote').innerHTML = model.TransNote;
                document.getElementById('info-status').innerHTML = model.StatusName;
                document.getElementById('info-amount').innerText = model.Amount;
                document.getElementById('info-flatfee').innerText = model.FlatFee;
                document.getElementById('info-realamount').innerText = model.RealAmount;
                document.getElementById('info-tocurrency').innerHTML = model.FromCurrency;
                document.getElementById('info-currencyfee').innerHTML = model.FromCurrency;
                document.getElementById('info-currencyreal').innerHTML = model.FromCurrency;
                document.getElementById('info-bankbene').innerHTML = model.BeneBank;
                //document.getElementById('info-branchbene').innerHTML = model.BeneBranch;
                $('#pharse-list tr').has('td').remove();
                if (model.PharseList.length > 0) {
                    for (var i = 0; i < model.PharseList.length; i++) {
                        var tr = document.getElementById('pharse-list').insertRow(i + 1);
                        var tdTranxDetailId = tr.insertCell(0);
                        tdTranxDetailId.innerHTML = model.PharseList[i].TranxDetailId;
                        var tdPharse = tr.insertCell(1);
                        tdPharse.innerHTML = model.PharseList[i].Pharse;
                        var tdResCode = tr.insertCell(2);
                        tdResCode.innerHTML = model.PharseList[i].ResCode;
                        var tdCreatedDate = tr.insertCell(3);
                        tdCreatedDate.innerHTML = model.PharseList[i].CreatedDate;
                        var tdTranxNote = tr.insertCell(4);
                        tdTranxNote.innerHTML = model.PharseList[i].TranxNote;
                    }
                }
                $('#viewDetail').modal("show");
            }
        });
    };


    $(document).keypress(function (e)
    {
        if (e.which == 13 && (e.target.id === "id-number" || e.target.id === "cus-name") || e.target.id === "mobile-number"
            || e.target.id === "branch" || e.target.id === "fromDate" || e.target.id === "toDate" || e.target.id === "status")
        {
            getList();
        }
    });
    function getList()
    {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/HistoryTransaction/GetList",
            params: {
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                mobile: JSON.stringify($('#mobile-number').val()),
                branch: JSON.stringify($('#branch').val()),
                service_type: JSON.stringify($('#service_type').val()),
                status: JSON.stringify($('#status').val()),
                service: JSON.stringify($('#service').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data)
        {
            if (redirecToLoginByTypeOf(data))
            {
                window.location.href = getRootUrl() + "/Error/Index";
            } else
            {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});


systemSearchApp.controller("hischargeController", function ($scope, $http) {
    getList();
    $scope.hisChargeSearch = function () {
        getList();
    };
    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "idnumber" || e.target.id === "cus-name") || e.target.id === "mobile-number"
            || e.target.id === "branch" || e.target.id === "fromDate" || e.target.id === "toDate" || e.target.id === "status") {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/ChargeHistory/GetListHistoryFee",
            params: {
                fromdate: JSON.stringify($('#fromDate').val()),
                todate: JSON.stringify($('#toDate').val()),
                mobile: JSON.stringify($('#mobile-number').val()),
                branch: JSON.stringify($('#branch').val()),
                cif: JSON.stringify($('#cif').val()),
                status: JSON.stringify($('#status').val()),
                idnumber: JSON.stringify($('#idnumber').val()),
                currency: JSON.stringify($('#currency').val()),
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = $scope.datas.length; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                $scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.datas.length / $scope.PerPageItems);
            }
        });
    }
});

systemSearchApp.controller("hisAccessAppController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.hisAccessAppSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };

    $scope.viewDetail = function (item) {
        $http({
            method: "post",
            url: getRootUrl() + "/MobileBankLog/ViewDetail",
            params: {

                id: JSON.stringify(item.Id),
            }
        }).then(function (msg) {

            if (redirectToLogin(msg)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                var model = msg.data;
                document.getElementById('info-mobile').innerHTML = model.MobileNo;
                document.getElementById('info-request').innerHTML = model.Request;
                document.getElementById('info-device').innerHTML = model.Device;
                document.getElementById('info-os').innerHTML = model.OS;
                document.getElementById('info-status').innerHTML = model.Status;
                document.getElementById('info-requestdate').innerHTML = model.RequestDate;
                document.getElementById('info-respondseddate').innerHTML = model.RespondsedDate;
                document.getElementById('info-desc').innerHTML = model.Description;
                document.getElementById('info-rescode').innerHTML = model.ResCode;
                document.getElementById('info-msgcode').innerHTML = model.MsgCode;
                document.getElementById('info-deviceid').innerHTML = model.DeviceID;
                $('#viewDetail').modal("show");
            }
        });
    };

    $(document).keypress(function (e) {
        if (e.which == 13 && (e.target.id === "request" || e.target.id === "mobile-number" ||
           e.target.id === "fromDate" || e.target.id === "toDate")) {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/MobileBankLog/GetList",
            params: {
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                mobile: JSON.stringify($('#mobile-number').val()),
                request: JSON.stringify($('#request').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});
systemSearchApp.controller("sessionAccessAppController", function ($scope, $http) {
    $scope.currentPage = 1;
    $scope.PerPageItems = 10;
    getList();
    $scope.sessionAccessAppSearch = function () {
        getList();
    };
    $scope.pageChanged = function () {
        getList();
    };
    $scope.kickOut = function (item) {

        var confirmMesg = "Bạn có chắc chắn muốn đóng phiên đăng nhập của tài khoản này?";
        var r = confirm(confirmMesg);
        if (r == true) {
            $http({
                method: "post",
                url: getRootUrl() + "/MobileBankSession/KickOut",
                params: {
                    id: JSON.stringify(item.Id),
                    session_str: JSON.stringify(item.Session_Str),
                    username: JSON.stringify(item.Mobile)
                }
            }).then(function (data) {
                if (redirectToLogin(data)) {
                    window.location.href = getRootUrl() + "/Error/Index";
                } else {
                    getList();
                }
            });;
        }
    }
    $(document).keypress(function (e) {
        if (e.which == 13 && e.target.id === "mobile-number") {
            getList();
        }
    });
    function getList() {
        var datas = $http({
            method: "get",
            url: getRootUrl() + "/MobileBankSession/GetList",
            params: {
                mobile: JSON.stringify($('#mobile-number').val()),
                frd: JSON.stringify($('#fromDate').val()),
                td: JSON.stringify($('#toDate').val()),
                pageIndex: $scope.currentPage,
                pageSize: $scope.PerPageItems
            }
        });
        datas.success(function (data) {
            if (redirecToLoginByTypeOf(data)) {
                window.location.href = getRootUrl() + "/Error/Index";
            } else {
                $scope.datas = data.records;
                $scope.filteredItems = $scope.datas.length;
                $scope.maxSize = 5;
                $scope.totalItems = data.total; //tổng số bản ghi trong danh sách.
                //$scope.PerPageItems = 10; //cấu hình số bản ghi trên 1 trang
                //$scope.currentPage = 1;
                $scope.numberPages = Math.ceil($scope.totalItems / $scope.PerPageItems);
            }
        });
    }
});

function redirectToLogin(data)
{
    if (data.data.toString().indexOf("!DOCTYPE") > 0)
    {
        return true;
    }
    return false;
}
function redirecToLoginByTypeOf(data)
{
    if (typeof data == "string")
    {
        return true;
    } else
    {
        return false;
    }
}

function setOption(data, id)
{
    var select = document.getElementById(id);
    var selectorSelect = "#" + id;
    $(selectorSelect).empty();
    for (var i = 0; i < data.length; i++)
    {
        var option = document.createElement("option");
        option.setAttribute("value", data[i]);
        var text = document.createTextNode(data[i]);
        option.appendChild(text);
        select.appendChild(option);
    }

}
function displayCurrency(val)
{
    if (isNaN(parseFloat(val)))
    {
        val = "";
    } else
    {
        val = parseFloat(val.replace(/,/g, ""))
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    return val;
}

