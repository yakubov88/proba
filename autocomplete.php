
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
// jquery 
 $('#searchPostedCertByCustomer').autocomplete({
                    type: 'POST',
                    dataType: 'JSON',
                    source: "controller/method",
                    select: function (event, ui) {
                        //        var strData = ui.item.value;
                        //        var data = strData.split('?');
                        //      $("#_company").val(data[0]);
                                $("#_city").val(ui.item.city);
                                $("#_address").val(ui.item.address);
                                $("#_phone").val(ui.item.phone);
                                $("#_fax").val(ui.item.fax);

                    }

                });
// controller      
 public function getCustomerAjax() {
        $term = $this->input->get('term', TRUE);
        $ajax = $this->CertificatModel->getCustomerNameAjax($term);
        $arrJSON = array();
        array_push($arrJSON, $term);
        foreach ($ajax as $value) {
            if (strlen($value['Name']) > 0) {
                array_push($arrJSON, mb_convert_encoding($value['Name'], 'UTF-8', "windows-1251"));
            }
        }
        header('Content-type: application/json');
        echo json_encode($arrJSON);
    }
    
    //model
    
    function getCustomerNameAjax($name) {
        $sql = "Select
                Distinct  [Alcomet\$Customer].Name
               From
                 [Alcomet\$Customer] Where [Alcomet\$Customer].Name like '" . $name . "%'";
        $query = $this->dbNav->query($sql);
        $rs = $query->result_array();
        return $rs;
    }
