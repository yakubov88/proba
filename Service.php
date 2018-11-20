<?php
/**
 * Created by PhpStorm.
 * User: amd
 * Date: 12.12.17
 * Time: 18:00
 */

class Service extends CI_Controller
{
    function __construct()
    {
        parent::__construct();
        $this->load->model('ServiceModel');      
        header('Access-Control-Allow-Origin: *');
    }

    function searchEmptyCell()
    {
        $method = $_SERVER['REQUEST_METHOD'];
        $arrResults = [];
        if ($method == 'POST') {
            $params = json_decode(file_get_contents('php://input'), TRUE);
            
            if (isset($params['coil'])) {
                $coil = $params['coil'];
                if (!$this->ServiceModel->coilExist($coil)) {
                    if($priority = $this->ServiceModel->coilPriority($coil)){  
						
                        $countPriority = count($priority);   
                        if($countPriority < 10){
                            $index = 10 -  $countPriority;
							
                            //   array_push($arrResults, $priority);                                                     
                            for ($i = 4; $i <= 30; $i++) {                                
                                $getTenCell = $this->ServiceModel->searchTenPriorityCell($i,$index);
                                if (count($getTenCell) < 10) {
                                    // array_push($arrResults, $getTenCell);
                                   $arrResults = array_merge($priority,$getTenCell);
                                } else {
                                    $arrResults = $getTenCell;
                                    break;
                                }
                            }                            
                        }else{
                            foreach($priority as $key => $row){
                                if ($key < 10) {
                                    array_push($arrResults, $row);
                                }else{
                                    $arrResults = $row;
                                    break;
                                }                              
                            }
                        }

                        $this->output
                                ->set_content_type('application/json')
                                ->set_output(json_encode(array('status' => 200, 'message' => $arrResults)));
                    }else{
						
                        for ($i = 4; $i <= 30; $i++) {
                            $getTenCell = $this->ServiceModel->searchTenCell($i);
                            if (count($getTenCell) < 10) {								
                                //array_push($arrResults, $getTenCell);
								 $arrResults = array_merge($arrResults,$getTenCell);
                            } else {								
								if($i == 4){
									$arrResults = $getTenCell;	
								}                          
                                break;
                            }
                        }
    
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 200, 'message' => $arrResults)));
                    }
                }else{
                    
                    $position = $this->ServiceModel->searchCoil($coil);                                        
                    $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 400, 'message' => 'Опаковка ' . $coil . ' вече е записанa, в клетка ' . $position[0]['cell'])));
                }
            } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Изисква се потребителско име или парола' )));
            }
        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }


    function login()
    {
        $method = $_SERVER['REQUEST_METHOD'];


        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

            if (isset($params['name']) && isset($params['password'])) {
                $name = $params['name'];
                $pass = $params['password'];
                if ($result = $this->ServiceModel->checkUser($name, $pass)) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => $result)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Потребителското име или паролата са неправилни')));
                }
            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Изисква се потребителско име или парола ' . var_dump($_REQUEST))));
            }


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }


    function register()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

            if (isset($params['name']) && isset($params['password']) && isset($params['permission'])) {

                $name = $params['name'];
                $pass = $params['password'];
                $permission = $params['permission'];

                if (!$this->ServiceModel->userNameExist($name)) {
                    if ($this->ServiceModel->makeRegister($name, $pass, $permission)) {
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 200, 'message' => 'Беше създаден потребител с име ' . $name)));
                    }

                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Потребител ' . $name . ' вече съществува')));
                }
            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Изисква се потребителско име и парола')));
            }


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }


    function allUser()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

            if (isset($params['id'])) {

                if ($result = $this->ServiceModel->allUser()) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => $result)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Няма регистрирани потребители')));
                }


            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Полето потребител е задължително')));
            }


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function countStatistic()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'GET') {
            $today = date("Y-m-d");
            $sevenDay = date('Y-m-d', strtotime('-7 days'));

            $countAllCoil = $this->ServiceModel->countAllCoil();
            $countAvgWeekCoil = $this->ServiceModel->countAvgWeekCoil($sevenDay);
            $sevDay = round($countAvgWeekCoil[0]['cnt'] / 7);
            $countEmptyCell = $this->ServiceModel->countEmptyCell();            
            $countBusyCell = $this->ServiceModel->countBusyCell();
            $countAllUsers = $this->ServiceModel->countAllUsers();
            $countWorker = $this->ServiceModel->countWorker();
//          $countAdmin = $this->ServiceModel->countAdmin();            
            $countEmptyPosition = $countEmptyCell - $countAllCoil[0]['cnt'];
            $result = array(
                (int)$countAllCoil[0]['cnt'],
                $sevDay,
                $countEmptyPosition,
                $countBusyCell,
                $countAllUsers,
                $countWorker
            );

            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 200, 'message' => $result)));


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function insertCoilInterval()
    {
        $method = $_SERVER['REQUEST_METHOD'];        
        if ($method == 'GET') {
            $startData = $this->uri->segment(3);
            $endData = $this->uri->segment(4);
            if (isset($startData) && isset($endData)) {
                $arr = $this->ServiceModel->insertCoilInterval($startData, $endData);
                $getArr = $this->ServiceModel->getCoilInterval($startData, $endData);
                $result = array('get' => $arr, 'insert' => $getArr);
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode($result));
            } else {

                $today = date("Y-m-d");
                $sevenDay = date('Y-m-d', strtotime('-7 days'));

                $arr = $this->ServiceModel->insertCoilInterval($sevenDay, $today);
                $getArr = $this->ServiceModel->getCoilInterval($sevenDay, $today);
                $result = array('get' => $arr, 'insert' => $getArr);
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode($result));
            }


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }




    function capacityByRow()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'GET') {
            $capacityByRowOne = $this->ServiceModel->capacityByRow(1);
            $capacityByRowTwo = $this->ServiceModel->capacityByRow(2);
            $capacityByRowThree = $this->ServiceModel->capacityByRow(3);
            $capacityByRowFour = $this->ServiceModel->capacityByRow(4);
            $capacityByRowFive = $this->ServiceModel->capacityByRow(5);


            $result = array(
                $capacityByRowOne,
                $capacityByRowTwo,
                $capacityByRowThree,
                $capacityByRowFour,
                $capacityByRowFive
            );

            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 200, 'message' => $result)));


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function capacityByProRow()
    {

        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'GET') {
            $countRowOne = 260;//$this->ServiceModel->countRows(1);
            $countRowTwo = 260;//$this->ServiceModel->countRows(2);
            $countRowThree = 260;//$this->ServiceModel->countRows(3);
            $countRowFour = 280;//$this->ServiceModel->countRows(4);
            $countRowFive = 280;//$this->ServiceModel->countRows(5);
        
            $capacityByRowOne = round(($this->ServiceModel->capacityByRow(1) / $countRowOne) * 100);
            $capacityByRowTwo = round(($this->ServiceModel->capacityByRow(2) / $countRowTwo) * 100);
            $capacityByRowThree = round(($this->ServiceModel->capacityByRow(3) / $countRowThree) * 100);
            $capacityByRowFour = round(($this->ServiceModel->capacityByRow(4) / $countRowFour) * 100);
            $capacityByRowFive = round(($this->ServiceModel->capacityByRow(5) / $countRowFive) * 100);


            $result = array(
                $capacityByRowOne,
                $capacityByRowTwo,
                $capacityByRowThree,
                $capacityByRowFour,
                $capacityByRowFive
            );

            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 200, 'message' => $result)));


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }


    }

    function deleteUser()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

            if (isset($params['id']) && isset($params['name'])) {
                $id = $params['id'];
                $name = $params['name'];
                if ($result = $this->ServiceModel->deleteUser($id)) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => 'Потребител ' . $name . ' беше изтрит')));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Няма регистрирани потребители')));
                }


            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Полето потребител е задължително')));
            }


        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function cellCapacity($cell_id, $cell){

        $cnt = $this->ServiceModel->cellCountCoilUnactive($cell_id);

        $capacity = $this->ServiceModel->cellCountCoil($cell);    
 
        if($cnt < $capacity[0]['capacity'] - 1){                        
            return true;            
        }else if($cnt == $capacity[0]['capacity'] - 1){ 
            return false;
        }else{            
            return false;
        }    

    }

    function insertCoilAndCell(){

        $method = $_SERVER['REQUEST_METHOD'];
     
        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);


            if ((isset($params['coil']) && strlen($params['coil']) > 0) && (isset($params['cell']) && strlen($params['cell']) > 0) && (isset($params['position']) && strlen($params['position']) > 0) && (isset($params['user_id']) && strlen($params['user_id']) > 0)) {
                    
                    $coil = trim($params['coil']);              
                    $cell = trim($params['cell']);
                    $position = $params['position'];
                    $user_id = $params['user_id'];
                if($this->ServiceModel->checkCoilNav($coil)){
                     
                    if (!$this->ServiceModel->coilExist($coil)) {
                        if ($this->ServiceModel->cellBusy($cell)) {
                            $cell_id = $this->ServiceModel->getCellId($cell);
                            
                                if($this->cellCapacity($cell_id ,$cell)){                                
                                        if ($this->ServiceModel->insertCoil($user_id, $cell_id, $coil, $position)) {
                                            $active = 0;
                                            if ($this->ServiceModel->makeActiveCell($cell,$active)) {     
                                                    $n_cell = $cell.'.'.$position;                                     
                                                    if($this->ServiceModel->makeRecordNav($coil, $n_cell)){
                                                       $this->output
                                                            ->set_content_type('application/json')
                                                            ->set_output(json_encode(array('status' => 200, 'message' => 'Опаковка ' . $coil . ' беше записанa в клетка ' . $cell.'.'.$position)));
                                                     }else{
                                                         $this->output
                                                             ->set_content_type('application/json')
                                                             ->set_output(json_encode(array('status' => 200, 'message' => '')));
                                                     }                                                                            
                                            }
                                        }
                                }else{
                                    $cell_id = $this->ServiceModel->getCellId($cell);
                                    
                                        if ($this->ServiceModel->insertCoil($user_id, $cell_id, $coil, $position)) {
                                            $active = 1;
                                            if ($this->ServiceModel->makeActiveCell($cell,$active)) {  
                                                    $n_cell = $cell.'.'.$position;                                      
                                                    if($this->ServiceModel->makeRecordNav($coil, $n_cell)){
                                                       $this->output
                                                            ->set_content_type('application/json')
                                                            ->set_output(json_encode(array('status' => 200, 'message' => 'Опаковка ' . $coil . ' беше записанa в клетка ' . $cell.'.'.$position)));
                                                    }else{
                                                        $this->output
                                                            ->set_content_type('application/json')
                                                            ->set_output(json_encode(array('status' => 200, 'message' => '')));
                                                    }                                                                            
                                            }
                                        }                        
                                }
                            }else{
                                $this->output
                                    ->set_content_type('application/json')
                                    ->set_output(json_encode(array('status' => 400, 'message' => 'Пълен капацитет или несъществуваща клетка'))); 
                            }
                        }else{

                            if($this->ServiceModel->getActiveCell($coil)){
                                $cell_old = $this->ServiceModel->getActiveCell($coil);
                            }else{
                                $cell_old = $this->ServiceModel->getCell($coil);
                            }
                            
                            $this->output
                                ->set_content_type('application/json')
                                ->set_output(json_encode(array('status' => 400, 'message' => 'Опаковка ' . $coil . ' вече е складиранa, в клетка '.$cell_old)));
                        }
                    
                }else{
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 400, 'message' =>"Невалидна опаковка, не съществува в Навижън")));
                } 
            }else{
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Задължителни са всички параметри')));
            }
        }else{
            $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!'))); 
        }
    }

    function insertInventoryCoilAndCell(){

        $method = $_SERVER['REQUEST_METHOD'];
     
        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);


            if ((isset($params['coil']) && strlen($params['coil']) > 0) && (isset($params['cell']) && strlen($params['cell']) > 0) && (isset($params['user_id']) && strlen($params['user_id']) > 0) && (isset($params['position']) && strlen($params['position']) > 0)) {
                    
                    $coil = trim($params['coil']);              
                    $cell = trim($params['cell']);
                    $position = $params['position'];
                    $user_id = $params['user_id'];
                    $ziro_cell_id = 457;
                 if($this->ServiceModel->checkCoilNav($coil)){
                    
                    if (!$this->ServiceModel->inventoryCoilExist($ziro_cell_id,$coil)) {
                        if ($this->ServiceModel->cellBusy($cell)) { 
                            $cell_id = $this->ServiceModel->getCellId($cell);
                            
                                if($this->cellCapacity($cell_id ,$cell)){
                                    if ($this->ServiceModel->removeCoilZiroCeil($ziro_cell_id, $coil)) {
                                        if ($this->ServiceModel->insertCoil($user_id, $cell_id, $coil, $position)) {
                                            $active = 0;
                                            if ($this->ServiceModel->makeActiveCell($cell,$active)) {    
                                                    $n_cell = $cell.'.'.$position;                                      
                                                    if($this->ServiceModel->makeRecordNav($coil, $n_cell)){                                                    
                                                        if(!$this->ServiceModel->existInZiro($ziro_cell_id, $coil)){
                                                            $this->ServiceModel->insertInventory($cell, $coil, $position);
                                                        }
                                                        $this->output
                                                            ->set_content_type('application/json')
                                                            ->set_output(json_encode(array('status' => 200, 'message' => 'Опаковка ' . $coil . ' беше записанa в клетка ' . $cell)));
                                                    
                                                     }else{
                                                         $this->output
                                                             ->set_content_type('application/json')
                                                             ->set_output(json_encode(array('status' => 200, 'message' => '')));
                                                     }                                                                            
                                            }
                                        }
                                    }else{
                                        $this->output
                                            ->set_content_type('application/json')
                                            ->set_output(json_encode(array('status' => 400, 'message' => 'Пакета не беше успешно изтрит'))); 
                                    }
                                }else{
                                    $cell_id = $this->ServiceModel->getCellId($cell);
                                    if ($this->ServiceModel->removeCoilZiroCeil($ziro_cell_id, $coil)) {    

                                        if ($this->ServiceModel->insertCoil($user_id, $cell_id, $coil, $position)) {
                                            $active = 1;
                                            if ($this->ServiceModel->makeActiveCell($cell,$active)) {        
                                                    $n_cell = $cell.'.'.$position;                                
                                                    if($this->ServiceModel->makeRecordNav($coil, $n_cell)){
                                                        if(!$this->ServiceModel->existInZiro($ziro_cell_id, $coil)){
                                                            $this->ServiceModel->insertInventory($cell, $coil, $position);
                                                        }
                                                        $this->output
                                                            ->set_content_type('application/json')
                                                            ->set_output(json_encode(array('status' => 200, 'message' => 'Опаковка ' . $coil . ' беше записанa в клетка ' . $cell)));
                                                    }else{
                                                        $this->output
                                                            ->set_content_type('application/json')
                                                            ->set_output(json_encode(array('status' => 200, 'message' => '')));
                                                    }                                                                            
                                            }
                                        }   
                                        
                                    }else{
                                        $this->output
                                            ->set_content_type('application/json')
                                            ->set_output(json_encode(array('status' => 400, 'message' => 'Пакета не беше успешно изтрит'))); 
                                    }
                                }
                            }else{
                                $this->output
                                    ->set_content_type('application/json')
                                    ->set_output(json_encode(array('status' => 400, 'message' => 'Пълен капацитет или несъществуваща клетка'))); 
                            }
                        }else{
                            $cell_old = $this->ServiceModel->getActiveCell($coil);
                            $this->output
                                ->set_content_type('application/json')
                                ->set_output(json_encode(array('status' => 400, 'message' => 'Опаковка ' . $coil . ' вече е складиранa, в клетка '.$cell_old)));
                        }
                    
                }else{
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 400, 'message' =>"Невалидна опаковка, не съществува в Навижън")));
                } 

            }else{
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Задължителни са всички параметри')));
            }
        }else{
            $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!'))); 
        }
    }

    function moveInOneCell(){
        $method = $_SERVER['REQUEST_METHOD'];
        if ($method == 'POST') {
            $params = json_decode(file_get_contents('php://input'), TRUE);
            if (isset($params['id'])){
                $cell_id = 457;
                if($cell_old = $this->ServiceModel->moveInOneCell($cell_id)){
                    if($this->ServiceModel->resetCapacity()){
                        $this->ServiceModel->deleteInventory();
                        $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => 'Успешно изместени опаковки в клетка 0.0.00.0')));
                    }
                }else{
                    $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Неуспешно местене')));
                }
            }else{
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Задължителни са всички параметри')));
            }
        }else{
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }


    function getAllCoilInCellActiveAndUnactive(){
        $method = $_SERVER['REQUEST_METHOD'];
        $result = array();
        if ($method == 'GET') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

               $arr = $this->ServiceModel->getAllCoilInCellActiveAndUnactive();

               foreach($arr  as $v){
                    
                    if($v['active'] == 1){
                        $active     =       'да';
                        $coil       =       $v['coil'];
                        $capacity   =       $v['capacity'];
                        $temp       =       $this->ServiceModel->getCountCoilInCell($v['cell']);
                        $count      =       $capacity - $temp;
                        $cell       =       $v['cell']; 
                        if($v['position'] != NULL){
                            $position   =       $v['position'];          
                        }else{
                            $position   =       0;
                        }         
                        
                    }else if($v['active'] == 0 && isset($v['coil'])){
                        $active     =       'не';
                        $coil       =       $v['coil'];
                        $count      =       '----';
                        $cell       =       $v['cell'];
                        $capacity   =       $v['capacity'];
                        if($v['position'] != NULL){
                            $position   =       $v['position'];          
                        }else{
                            $position   =       0;
                        }         
                    }else{
                        $coil       =       '----';
                        $active     =       '----';
                        $count      =       '----';
                        $cell       =       $v['cell'];
                        $capacity   =       $v['capacity'];
                        if($v['position'] != NULL){
                            $position   =       $v['position'];          
                        }else{
                            $position   =       0;
                        }         
                    } 
                    
                    if($cell != '0.0.00.0'){
                        $temp  =  array(
                            // 'coil_id'       =>      $v['coil_id'],
                            'coil'          =>      $coil,
                            'active'        =>      $active,
                            'cell'          =>      $cell,
                            'capacity'      =>      $capacity,
                            'count'         =>      $count,
                            'position'      =>      $position
                        );
                        array_push($result, $temp);
                    }                   
    
                }
                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => $result)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Няма записи')));
                }
   
        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }
    
    function getAllCoilInCell(){

        $result = array();
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'GET') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

                $arr = $this->ServiceModel->getAllCoilInCell();

                foreach($arr  as $v){
                    
                        $coil       =       $v['coil'];
                        $capacity   =       $v['capacity'];
                        $temp       =       $this->ServiceModel->getCountCoilInCell($v['cell']);
                        $count      =       $capacity - $temp;
                        $cell       =       $v['cell'];   
                        $position   =       $v['position'];                        
                    
                    
                    
                    
                    $temp  =  array(                    
                        'coil'          =>      $coil,                    
                        'cell'          =>      $cell,
                        'capacity'      =>      $capacity,
                        'position'      =>      $position,
                        'count'         =>      $count
                    );
                    array_push($result, $temp);
                }

                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => $result)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Няма записи')));
                }

     

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function getAllZiroCoilInCell(){
        $result = array();
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'GET') {

            $params = json_decode(file_get_contents('php://input'), TRUE);
                $ziro_id = 457;
                $ziroArr = $this->ServiceModel->getAllZiroCoilInCell($ziro_id);
                $unknowCoil = $this->ServiceModel->getUnknowCoil();
                if($unknowCoil){
                    $result = array_merge($ziroArr,$unknowCoil);
                }else{
                    $result = $ziroArr;
                }

            if ($result) {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 200, 'message' => $result)));
            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 0)));
            }    

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function changeWeight(){
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);


            if (isset($params['weight']) && isset($params['id'])) {
                $weight = $params['weight'];
                $id = $params['id'];
                $result = $this->ServiceModel->changeWeight($id, $weight);
                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => 'successful weight change, current weight is '. $weight)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'unsuccessful weight change')));
                }

            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Weight is required')));
            }

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }
    
    function changeCapacity(){
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);


            if (isset($params['capacity']) && isset($params['id'])) {
                $capacity = $params['capacity'];
                $id = $params['id'];
                $result = $this->ServiceModel->changeCapacity($id, $capacity);
                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => 'Капацитета беше променен на '. $capacity)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Помяната на капацитет беше неуспешна')));
                }

            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Полето капацитет е задължително')));
            }

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function changePriority(){
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {
            $params = json_decode(file_get_contents('php://input'), TRUE);

            if (isset($params['priority']) && isset($params['id'])) {
                $priority = $params['priority'];
                $id = $params['id'];
                $result = $this->ServiceModel->changePriority($id, $priority);
                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => 'successful capacity change, current priority is '. $priority)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'unsuccessful priority change')));
                }

            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Priority is required')));
            }

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function getCellWeight(){
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);


            if (isset($params['cell'])) {
                $cell = $params['cell'];
                $result = $this->ServiceModel->searchCell($cell);
                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => $result)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Клетка ' . $cell . ' не съществува в склад')));
                }

            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Полето клетка е задължително')));
            }

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function searchCoil()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);


            if (isset($params['coil'])) {
                $coil = $params['coil'];
                $result = $this->ServiceModel->searchCoil($coil);
                if ($result) {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => $result)));
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Опаковка ' . $coil . ' не е складиранa')));
                }

            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'Полето рулон е задължително')));
            }

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    function takeCoil()
    {
        $method = $_SERVER['REQUEST_METHOD'];

        if ($method == 'POST') {

            $params = json_decode(file_get_contents('php://input'), TRUE);

            if (isset($params['coil_id']) && isset($params['cell_id'])) {

                $coil_id = $params['coil_id'];
                $cell_id = $params['cell_id'];
                $coil = $params['coil'];
                $cell = $params['cell'];
                $position = $params['position'];

                if ($this->ServiceModel->takeCoil($coil_id)) {
                    if ($this->ServiceModel->deactiveCell($cell_id)) {
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 200, 'message' => 'Опаковка ' . $coil . ' беше взетa от ' . $cell.'.'.$position)));
                    }
                } else {
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => 'Опаковка ' . $coil . ' несъществува в склад')));
                }
            } else {
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'рулон и клетка са задължителни полета')));
            }

        } else {
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!')));
        }
    }

    /*
     *
     * Inventory start
     *
     */
    
    function getVersion(){
        $method = $_SERVER['REQUEST_METHOD'];
        $params = json_decode(file_get_contents('php://input'), TRUE); 
        if ($method == 'GET') {
            $version = $this->ServiceModel->getVersion();
            $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 200, 'message' => $version)));                 
        }
    }

    function getChanges(){
        $method = $_SERVER['REQUEST_METHOD'];
        $params = json_decode(file_get_contents('php://input'), TRUE);
        
        if ($method == 'GET') {
            $version = $this->ServiceModel->getVersion();
            
            if($arr = $this->ServiceModel->getChanges($version)){
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 200, 'message' => $arr)));
            }else{
                $this->output
                    ->set_content_type('application/json')
                    ->set_output(json_encode(array('status' => 400, 'message' => 'No results')));
            }
        }
    }

    function inventorySync(){
        $method = $_SERVER['REQUEST_METHOD'];
        $params = json_decode(file_get_contents('php://input'), TRUE); 
        
        if ($method == 'POST') {
                if(isset($params['user_id'])){
                    if($arr = $this->ServiceModel->getCurrnetCellCoil()){
                        $this->ServiceModel->deleteOldInventory();
                        $version = $this->ServiceModel->getVersion() + 1;
                        foreach($arr  as $v){                        
                            $this->ServiceModel->insertInventory($v['coil_id'],$v['cell_id'],$v['cell'],$v['coil'], $version);
                        }  
                         
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 200, 'message' => 'Synchronice is done!', 'value' => $version)));                 
                    }else{
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 400, 'message' => 'The table is empty')));
                    }
            }else{
                $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'User is required')));
            }
        }else{
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!'))); 
        }
    }

    function checkSync(){
        $method = $_SERVER['REQUEST_METHOD'];
        $params = json_decode(file_get_contents('php://input'), TRUE); 
        
        if ($method == 'POST') {
            if(isset($params['user_id'])){
                $arr = $this->ServiceModel->getCurrnetCellCoil();               
                if(count($arr)==0){
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 200, 'message' => 'The table is empty')));
                }else{
                    $this->output
                        ->set_content_type('application/json')
                        ->set_output(json_encode(array('status' => 400, 'message' => '')));
                }
            }else{
                $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'User is required')));
            }
        }else{
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!'))); 
        }
    }



    function  overWriteRecord(){
        $method = $_SERVER['REQUEST_METHOD'];
        $params = json_decode(file_get_contents('php://input'), TRUE); 
        
        if ($method == 'POST') {            
                if(isset($params['coil']) && strlen($params['coil'])>0){
                    $coil = $params['coil'];
                    $cell = $params['cell'];
                    if($arr = $this->ServiceModel->overWriteRecord($coil)){

                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 200, 'message' => 'Coil ' . $arr[0]['coil'] . ' in ' .  $arr[0]['cell'] . ', if you want change position click on the button')));                 
                    }else{
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 400, 'message' => 'Cell and Coil is not correct position')));
                    }
            }else{
                $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Coil is required')));
            }
        }else{
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!'))); 
        }
    }

    function  checkCoilCell(){
        $method = $_SERVER['REQUEST_METHOD'];
        $params = json_decode(file_get_contents('php://input'), TRUE); 
        
        if ($method == 'POST') {            
                if(isset($params['coil']) && strlen($params['coil']) > 0 && isset($params['cell'])  && strlen($params['cell'])>0){
                    
                    $coil = $params['coil'];
                    $cell = $params['cell'];
                    if($arr = $this->ServiceModel->existCell($cell)){
                        if($arr = $this->ServiceModel->checkCoilCell($coil, $cell)){
                            
                            $this->output
                                ->set_content_type('application/json')
                                ->set_output(json_encode(array('status' => 200, 'message' => 'Cell and Coil is correct position','value'=> false)));                 
                        }else{
                            
                            $this->output
                                ->set_content_type('application/json')
                                ->set_output(json_encode(array('status' => 400, 'message' => 'Cell and Coil is not correct position','value'=> true)));
                        }
                    }else{
                        
                        $this->output
                            ->set_content_type('application/json')
                            ->set_output(json_encode(array('status' => 400, 'message' => 'Opss, Cell does not exist', 'value'=> false)));
                    }
            }else{
                $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Coil and Cell is required','value'=> false)));
            }
        }else{
            $this->output
                ->set_content_type('application/json')
                ->set_output(json_encode(array('status' => 400, 'message' => 'Лоша заявка!','value'=> false))); 
        }
    }

  
   function generateCell()
   {

    // $arr = $this->ServiceModel->getCellNav();

    // if($arr){
    //     foreach($arr as $val){
    //         $sum = 0;
    //         $arr = explode('.', $val['Code']);
    //         $sum = $arr[0] + $arr[1] + $arr[2] + $arr[3];
           
    //         if(!$this->ServiceModel->existCell($val['Code'])){
    //              $this->ServiceModel->insertCell($val['Code'], $sum);
    //         }else{
    //             $this->output
    //                 ->set_content_type('application/json')
    //                 ->set_output(json_encode(array('status' => 400, 'message' => 'Opss, Cell Exist ' . $val)));
    //         }
    //     }
    // }else{
    
    //     $this->output
    //         ->set_content_type('application/json')
    //         ->set_output(json_encode(array('status' => 400, 'message' => 'NQMA POLETA'))); 
    // }

    // $method = $_SERVER['REQUEST_METHOD'];
    // $params = json_decode(file_get_contents('php://input'), TRUE); 


        // if ($method == 'POST') {          
            // if(isset($params['warehouse']) && isset($params['stage']) && isset($params['row']) && isset($params['floor'])){

                // $warehouse = $params['warehouse'];
                // $stage = $params['stage'];
                // $row = $params['row'];
                // $floor = $params['floor'];

                // $warehouse = 5;
                // $stage = 5;
                // $row = 25;
                // $floor = 5;

                // for ($w = 1; $w <= $warehouse; $w++) {
                //     for ($s = 1; $s <= $stage; $s++) {
                //         for ($r = 1; $r <= $row; $r++) {
                //             for ($f = 1; $f <= $floor; $f++) {

                //                 switch (strlen($r)) {
                //                     case 1:
                //                         $ziro = '0';
                //                         break;                
                //                     default:
                //                         $ziro = '';
                //                         break;
                //                 }

                //                 $val = $w.'.'.$s.'.'.$ziro.$r.'.'.$f;
                //                     if(!$this->ServiceModel->existCell($val)){
                //                         $this->ServiceModel->insertCell($val, $f);
                //                     }else{
                //                         $this->output
                //                         ->set_content_type('application/json')
                //                         ->set_output(json_encode(array('status' => 400, 'message' => 'Opss, Cell Exist ' . $val)));
                //                     }
                                
                //             }
                //         }
                //     }
                // }
                // $this->output
                //     ->set_content_type('application/json')
                //     ->set_output(json_encode(array('status' => 200, 'message' => 'Cells is done')));
            // }else{
            //     $this->output
            //     ->set_content_type('application/json')
            //     ->set_output(json_encode(array('status' => 400, 'message' => 'Opss, Warehouse, Stage, Row and Floor is required')));
            // }
        // }else{
        //     $this->output
        //         ->set_content_type('application/json')
        //         ->set_output(json_encode(array('status' => 400, 'message' => 'Bed Request!'))); 
        // }

   }


   function fixRow(){

       $arr = $this->ServiceModel->getAllCell();
       foreach($arr as $v){
        $arr = explode('.', $v['cell']);
        
        $this->ServiceModel->updateFix($v['cell_id'],  $arr[3]);
       }
   }

}