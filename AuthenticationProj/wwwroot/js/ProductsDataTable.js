$(document).ready(function () {
    $('#Products').dataTable({
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/Products",
            "type": "POST",
            "dataType": "json"
        },
        "columsDefs": [{
            "targets": [0],
            "visible": false,
            "searchable":false
        }],
        "columns": [
            {"data":"id","name":"Id","autowidth":true},
            { "data": "name", "name":"Name","autowidth":true},
            { "data": "brand", "name":"Brand","autowidth":true},
            { "data": "model", "name":"Model","autowidth":true},
            { "data": "productionDate", "name":"ProductionDate","autowidth":true},
            { "data": "subCategoryId", "name":"SubCategoryId","autowidth":true},
            { "data": "price", "name":"Price","autowidth":true},
            { "data": "sellerName", "name": "SellerName", "autowidth": true },
            { "render": function (data, type, row) { return '<a href="#" class="btn btn-primary" onclick=Details("' + row.id + '");> Details </a>' },"orderable":false }
            //{ "render": function (data, type, row) { return '<a href="#" class="btn btn-warning" onclick=Edit("' + row.id + '");> Edit </a>' }, "orderable": false },
            //{ "render": function (data, type, row) { return '<a href="#" class="btn btn-danger" onclick=DeleteCustomer("' + row.id + '");>Delete </a>' }, "orderable": false },
        ]
    });
});