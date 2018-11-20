<?php
/**
 * Created by PhpStorm.
 * User: amd
 * Date: 12.12.17
 * Time: 18:01
 */

class ServiceModel extends CI_Model
{
    function __construct()
    {
        $dbNav;
		$db;
		
        parent::__construct();	
		$this->db = $this->load->database('default', TRUE);
        $this->dbNav = $this->load->database('Navision', TRUE);
        // echo $this->db->last_query();
    }

    function searchTenCell($row)
    {
        $sql = "SELECT `cell_id`,`cell` FROM `cell` WHERE `weight`='$row' AND `active`=0 ORDER BY weight LIMIT 0,12";
        $query = $this->db->query($sql);
        $rs = $query->result_array();
        return $rs;
    }

    function searchTenPriorityCell($row, $index){
        $sql = "SELECT `cell_id`,`cell` FROM `cell` WHERE `weight`='$row' AND `active`=0 LIMIT 0,$index";
        $query = $this->db->query($sql);
        $rs = $query->result_array();
        return $rs;
    }


    function searchCoil($coil)
    {

        $this->db->select('coil.coil_id, coil.cell_id, coil.coil, coil.position, coil.date,cell.cell')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')
            ->where('coil.coil', $coil)
            ->where('coil.active', 1);

        $query = $this->db->get();
        $result = $query->result_array();
        return $result;
    }

    function cellCountCoilUnactive($cell){
        // $this->db->select('coil.coil, cell.cell_id, cell.cell, cell.weight, cell.capacity')->from('cell')            
        //     ->join('coil', 'coil.cell_id=cell.cell_id', 'LEFT')    
        //     ->where('cell.cell', $cell)
        //     ->where('coil.active', 0);
        $this->db->select('coil')->from('coil')            
        ->where('cell_id', $cell)
        ->where('coil.active', 1);


        $query = $this->db->get();
        $result = $query->result_array();        
        return count($result); 
    }

    function cellCountCoil($cell){
        $this->db->select('coil.coil, cell.cell_id, cell.cell, cell.weight, cell.capacity')->from('cell')            
            ->join('coil', 'coil.cell_id=cell.cell_id', 'LEFT')    
            ->where('cell.cell', $cell);

        $query = $this->db->get();
        $result = $query->result_array();
        return $result; 
    }

    function searchCell($cell){
        $this->db->select('cell.cell_id, cell.cell, cell.weight, cell.capacity, cell.priority')->from('cell')            
            ->where('cell.cell', $cell)
            ->limit(1);

        $query = $this->db->get();
        $result = $query->result_array();
        return $result; 
    }

    function getCell($coil){
        $this->db->select('cell.cell, coil.position')->from('cell')            
        ->join('coil', 'coil.cell_id=cell.cell_id', 'LEFT')    
        ->where('coil.coil', $coil);

        $query = $this->db->get();
        $result = $query->result_array();

        if($result[0]['position'] == 0){
            $position = 'x';
        }else{
            $position = $result[0]['position'];
        }

        return $result[0]['cell'].'.'.$position; 
    }

    function getActiveCell($coil){
        $this->db->select('cell.cell, coil.position')->from('cell')            
        ->join('coil', 'coil.cell_id=cell.cell_id', 'LEFT')          
        ->where('coil.coil', $coil)
        ->where('coil.active', 1);

        $query = $this->db->get();
        $result = $query->result_array();        
        if($result[0]['position'] == 0){            
            $position = 'x';
        }else{            
            $position = $result[0]['position'];
        }

        return $result[0]['cell'].'.'.$position;
    }

    function getAllCoilInCellActiveAndUnactive()
    {
        $this->db->select('coil.coil_id, coil.coil, coil.active, coil.date, coil.position, cell.cell_id, cell.cell, cell.weight, cell.priority, cell.capacity, cell.active as act')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'RIGHT')                       
            ->order_by('cell.cell', 'ASC');

        $query = $this->db->get();
        $result = $query->result_array();
        return $result;
    }

    function getCountCoilInCell($cell){
        $this->db->select('count(coil.coil_id) as cnt')->from('coil')
        ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')
        ->where('cell.cell', $cell)
        ->where('coil.active', 1);

    $query = $this->db->get();
    $result = $query->result_array();
    return $result[0]['cnt'];
    }

    function getAllCoilInCell()
    {
        $this->db->select('coil.coil, cell.cell, cell.capacity, coil.position')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')            
            ->where('coil.active', 1)
            ->order_by('coil.date', 'DESC');

        $query = $this->db->get();
        $result = $query->result_array();
        return $result;
    }

    function getAllZiroCoilInCell($id)
    {
        $this->db->select('coil.coil, cell.cell')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')            
            ->where('cell.cell_id', $id)
            ->where('coil.active', 1);

        $query = $this->db->get();
        $result = $query->result_array();
        return $result;
    }

    function deleteInventory(){
        $this->db->empty_table('inventory');
        return true;
    }

    function existInZiro($cell_id, $coil)
    {
        $this->db->select('coil.coil')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')      
            ->where('cell.cell_id', $cell_id)      
            ->where('coil.coil', $coil);

            $query = $this->db->get();
            $result = $query->result_array();
            if($result){
                return true;
            }
            return false;  
    }

    function getCurrnetCellCoil()
    {
        $this->db->select('coil.coil_id, coil.cell_id, coil.coil, coil.date,cell.cell')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')
            ->where('coil.active', 1);

        $query = $this->db->get();
        $result = $query->result_array();
        return $result;
    }

    function checkUser($name, $pass)
    {
        $this->db->select('user_id, name, permission')->from('users')
            ->where('name', $name)
            ->where('password', md5($pass))
            ->where('active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return $result;
        }
        return false;
    }

    function allUser()
    {
        $this->db->select('user_id, name, real_pass as pass, permission')->from('users')->where('active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return $result;
        }
        return false;
    }

    function countAllCoil()
    {
        $this->db->select('count(user_id) as cnt')->from('coil')->where('active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return $result;
        }
        return false;
    }

    function makeRecordNav($coil, $cell){		
    //OLD:     return $this->dbNav->query("UPDATE LotNo SET LotNo.[Current Zone Code] = '$cell' WHERE LotNo.[Lot No_] = '$coil'"); 
    
    return $this->dbNav->query("UPDATE dbo.LotNo SET [Current Zone Code] = '$cell' WHERE [Lot No_] = '$coil'");
    }

    // function getCellNav(){        
    //     $sql = "Select
    //                 RollingLocationZone.Code
    //             From
    //                 RollingLocationZone";

    //     $query = $this->dbNav->query($sql);
    //     $rs = $query->result_array();
    //     return $rs;
    // }


    function checkCoilNav($coil){
    
    
    //OLD     $sql = "Select
    //         LotNo.[Lot No_]
    //         From
    //         LotNo
    //         Where
    //         LotNo.[Lot No_] = '$coil'";

       $sql = "select [Lot No_] from dbo.LotNo where [Lot No_]='$coil'";
        $query = $this->dbNav->query($sql);
        $rs = $query->result_array();
        if(count($rs)>0){
            return true;
        }
        return false;
    }

    function insertCoilInterval($startData, $endData)
    {

        $sql = "SELECT COUNT(user_id) as cnt, SUBSTRING_INDEX(`date`, ' ', 1) as date FROM `coil` WHERE SUBSTRING_INDEX(`date`, ' ', 1) >= '$startData' AND SUBSTRING_INDEX(`date`, ' ', 1) <= '$endData' AND `active` = 1 GROUP BY SUBSTRING_INDEX(`date`, ' ', 1)";
        $query = $this->db->query($sql);
        $rs = $query->result_array();
        return $rs;

    }

    function insertInventory($cell, $coil, $position)
    {
        $date = date('Y-m-d H:i:s', time());
        $data = array(            
            'cell' => $cell,
            'coil' => $coil,            
            'position' => $position,            
            'date' => $date            
        );

        $this->db->set($data);
        if ($this->db->insert('inventory')) {
            return TRUE;
        }
        return FALSE;
    }


    function insertRowInventory($old_coil_id, $old_cell_id, $cell, $old_coil, $coil, $version){
        $date = date('Y-m-d H:i:s', time());
        $data = array(
            'coil_id' => $old_coil_id,
            'cell_id' => $old_cell_id,
            'cell' => $cell,
            'coil' => $old_coil,
            'new_cell' => $cell,
            'new_coil' => $coil,
            'version' => $version,
            'date' => $date,
            'active' => 1
        );

        $this->db->set($data);
        if ($this->db->insert('inventory')) {
            return TRUE;
        }
        return FALSE;
    }

    function getCoilInterval($startData, $endData)
    {

        $sql = "SELECT COUNT(user_id) as cnt, SUBSTRING_INDEX(`date`, ' ', 1) as date FROM `coil` WHERE SUBSTRING_INDEX(`date`, ' ', 1) >= '$startData' AND SUBSTRING_INDEX(`date`, ' ', 1) <= '$endData' AND `active` = 0 GROUP BY SUBSTRING_INDEX(`date`, ' ', 1)";
        $query = $this->db->query($sql);
        $rs = $query->result_array();
        return $rs;
    }

    function countAvgWeekCoil($sevenDay)
    {

        $this->db->select('count(coil) as cnt')->from('coil')
            ->where('active', 1)
            ->where('date >=', $sevenDay);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return $result;
        }
        return false;
    }

    function countEmptyCell()
    {

        $this->db->select('sum(`capacity`) as cnt')->from('cell');
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return (int)$result[0]['cnt'];
        }
        return false;
    }

    function countBusyCell()
    {

        $this->db->select('count(coil) as cnt')->from('coil')
            ->where('active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return (int)$result[0]['cnt'];
        }
        return false;
    }

    function countAllUsers()
    {

        $this->db->select('count(user_id) as cnt')->from('users')
            ->where('active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return (int)$result[0]['cnt'];
        }
        return false;
    }

    function countRows($row){
        $this->db->select('count(cell_id) as cnt')->from('cell')      
            ->where('cell.weight', $row);
        $query = $this->db->get();

        if ($result = $query->result_array()) {
            // echo $this->db->last_query();
            // exit;
            return $result[0]['cnt'];
        }
        return false;
    }

    function capacityByRow($row)
    {
        $this->db->select('count(coil.coil_id) as cnt')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')
            ->where('cell.weight', $row)
            ->where('coil.active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {
            return $result[0]['cnt'];
        }
        return false;
    }

    function coilPriority($coil)
    {
        $this->db->select('`cell_id`,`cell`')->from('cell')
            ->where('active', 0)
            ->where('priority', 1)
            ->limit(10);
        $query = $this->db->get();

        if ($result = $query->result_array()) {
            return $result;
        }
        return false;
    }

    function countWorker()
    {

        $this->db->select('count(user_id) as cnt')->from('users')
            ->where('permission', 0)
            ->where('active', 1);
        $query = $this->db->get();

        if ($result = $query->result_array()) {

            return (int)$result[0]['cnt'];
        }
        return false;
    }


    function checkCoilCell($coil, $cell)
    {
        // $this->db->select('inventory_id')->from('inventory')
        //     ->where('cell', $cell)
        //     ->where('coil', $coil)
        //     ->where('active', 1);
        // $query = $this->db->get();

        // if ($result = $query->result_array()) {
        //     return (int)$result[0]['inventory_id'];
        // }
        // return false;
        $this->db->select('coil.coil_id, coil.cell_id, coil.coil, coil.date,cell.cell')->from('coil')
        ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')
        ->where('coil.coil', $coil)
        ->where('cell.cell', $cell)
        ->where('coil.active', 1);

        $query = $this->db->get();
        $result = $query->result_array();
        if($result){
            return true;
        }
        return false;    
    }

    function overWriteRecord($coil){
        $this->db->select('coil.cell_id, coil.coil_id, coil.coil, cell.cell')->from('coil')
            ->join('cell', 'coil.cell_id=cell.cell_id', 'LEFT')
            ->where('coil.coil', $coil)
            ->where('coil.active', 1);        
        $query = $this->db->get();
        $result = $query->result_array();
        return $result;
    }

    function getChanges($version){
        $this->db->select('cell, coil, new_coil')->from('inventory')           
            ->where('version', $version)
            ->where('CHAR_LENGTH(new_coil) >', 0);

        $query = $this->db->get();        
        $result = $query->result_array();        
        return $result;
    }

    function existCellInInventory($id, $version){
        $this->db->select('cell_id')->from('inventory')           
            ->where('version', $version)
            ->where('cell_id', $id);

        $query = $this->db->get();        
        $result = $query->result_array(); 
        if(isset($result) && count($result)>0){
            return $result;
        }   
        return false;
    }

    function recordNewPosition($coil, $cell_id)
    {
        $data = array(
            'coil' => $coil
        );
        $this->db->where('cell_id', $cell_id);
        if ($this->db->update('coil', $data)) {           
            return TRUE;
        }
        return FALSE;
    }

    function moveInOneCell($cell_id){
        $data = array(
            'cell_id' => $cell_id
        );
        $this->db->where('active', 1);
        if ($this->db->update('coil', $data)) {           
            return TRUE;
        }
        return FALSE;
    }

    function resetCapacity(){
        $data = array(
            'active' => 0
        );

        if ($this->db->update('cell', $data)) {           
            return TRUE;
        }
        return FALSE;
    }

    function activeCell($cell_id){
        $data = array(
            'active' => 0
        );
        $this->db->where('cell_id', $cell_id);
        if ($this->db->update('cell', $data)) {
            return TRUE;
        }
    }

    function removeCoilZiroCeil($cell_id, $coil){
        $data = array(
            'active' => 0
        );
        $this->db->where('cell_id', $cell_id);
        $this->db->where('coil', $coil);
        if ($this->db->update('coil', $data)) {
            // echo $this->db->last_query();
            // exit;
            return TRUE;
        }
        
        return false;
    }

    function removeCellAndCoil($cell_id, $coil){
        $data = array(
            'active' => 0
            // 'coil' => ''
        );
        $this->db->where('cell_id', $cell_id);
        $this->db->where('coil', $coil);
        if ($this->db->update('coil', $data)) {
            return TRUE;
        }
        return false;
    }

    function removeCoil($coil){
        $data = array(
            'active' => 0            
        );        
        $this->db->where('coil', $coil);
        if ($this->db->update('coil', $data)) {
            return TRUE;
        }
        return false;
    }

    function changeWeight($cell_id, $weight)
    {
        $data = array(
            'weight' => $weight
        );
        $this->db->where('cell_id', $cell_id);
        if ($this->db->update('cell', $data)) {
            return TRUE;
        }
        return FALSE;
    }

    function changeCapacity($cell_id, $capacity)
    {
        $data = array(
            'capacity' => $capacity
        );
        $this->db->where('cell_id', $cell_id);
        if ($this->db->update('cell', $data)) {
            return TRUE;
        }
        return FALSE;
    }


    function changePriority($cell_id, $priority){
        $data = array(
            'priority' => $priority
        );
        $this->db->where('cell_id', $cell_id);
        if ($this->db->update('cell', $data)) {
            return TRUE;
        }
        return FALSE;
    }

    function deleteUser($id)
    {
        $data = array(
            'active' => 0
        );
        $this->db->where('user_id', $id);
        if ($this->db->update('users', $data)) {
            return TRUE;
        }
        return FALSE;
    }


    function deleteOldInventory()
    {
        $data = array(
            'active' => 0
        );
        $this->db->where('active', 1);
        if ($this->db->update('inventory', $data)) {
            return TRUE;
        }
        return FALSE;
    }

    function userNameExist($name)
    {
        $this->db->select('name')->from('users')
            ->where('name', $name)
            ->where('active', 1);
        $query = $this->db->get();
        if ($query->result_array()) {
            return true;
        }
        return false;
    }

    function existCell($cell)
    {
        $this->db->select('cell')->from('cell')
            ->where('cell', $cell);
                       
        $query = $this->db->get();
        if ($query->result_array()) {
            return true;
        }
        return false;
    }


    function cellBusy($cell)
    {
        $this->db->select('cell')->from('cell')
            ->where('cell', $cell)
            ->where('active', 0);
        $query = $this->db->get();
        if ($query->result_array()) {
            return true;
        }
        return false;
    }

    function coilExist($coil)
    {
        $this->db->select('coil')->from('coil')
            ->where('active', 1)
            ->where('coil', $coil);
        $query = $this->db->get();
        if ($query->result_array()) {
            return true;
        }
        return false;
    }

    function inventoryCoilExist($cell_id,$coil)
    {
        $this->db->select('coil')->from('coil')
            ->where('active', 1)
            ->where('coil', $coil)
            ->where('cell_id !=', $cell_id);
        $query = $this->db->get();
        // echo $this->db->last_query();
        // exit;
        if ($query->result_array()) {
            return true;
        }
        return false;
    }

    function getUnknowCoil(){

        $this->db->select('cell, coil')->from('inventory');

        $query = $this->db->get();
        if ($result = $query->result_array()) {            
            return $result;
        }   
    }

    function getVersion(){
        $this->db->select_max('version')->from('inventory')
        ->limit(1);
       
        $query = $this->db->get();    
        if ($result = $query->result_array()) {
           
            return $result[0]['version'];
        }  
    }

    function getCellId($cell){        
        $this->db->select('cell_id')->from('cell')
            // ->where('active', 1)
            ->where('cell', $cell);
        $query = $this->db->get();
        if ($result = $query->result_array()) {            
            return $result[0]['cell_id'];
        }    
    }

    function chackCellCapacity($id){
        $this->db->select('capacity')->from('cell')            
            ->where('cell_id', $id);
        $query = $this->db->get();
        if ($result = $query->result_array()) {                 
            return $result[0]['capacity'];
        }           
    }

    function cellCountCoilById($id){
        $this->db->select('coil.coil, cell.cell_id, cell.cell, cell.weight, cell.capacity')->from('cell')            
            ->join('coil', 'coil.cell_id=cell.cell_id', 'LEFT')    
            ->where('cell.cell_id', $id);

        $query = $this->db->get();
        $result = $query->result_array();
        return $result; 
    }

    function makeActiveCell($cell, $active)
    {
        $data = array(
            'active' => $active
        );
        $this->db->where('cell', $cell);
        if ($this->db->update('cell', $data)) {
            return TRUE;
        }
        return FALSE;
    }

    function takeCoil($coil)
    {
        $data = array(
            'active' => 0
        );
        $this->db->where('coil_id', $coil);
        if ($this->db->update('coil', $data)) {
            return TRUE;
        }
        return FALSE;
    }

    function updateInventory($cell_id, $cell, $coil, $version){
        
        $date = date('Y-m-d H:i:s', time());
        $data = array(
            'new_cell' => $cell,
            'new_coil' => $coil,
            'date'  => $date            
        );
        $this->db->where('version', $version);
        $this->db->where('cell_id', $cell_id);
        if ($this->db->update('inventory', $data)) {            
            return TRUE;
        }

        return FALSE;
    }

    function deactiveCell($cell)
    {
        $data = array(
            'active' => 0
        );
        $this->db->where('cell_id', $cell);
        if ($this->db->update('cell', $data)) {
            return TRUE;
        }
        return FALSE;
    }

    function insertCell($cell, $weight)
    {

        $data = array(
            'cell' => $cell,
            'weight' => $weight
        );

        $this->db->set($data);
        if ($this->db->insert('cell')) {
            return TRUE;
        }
        return FALSE;
    }

    function insertCoil($user_id, $cell_id, $coil, $position)
    {

        $date = date('Y-m-d H:i:s', time());

        $data = array(
            'cell_id' => $cell_id,
            'user_id' => $user_id,
            'coil' => $coil,
            'position' => $position,
            'active' => 1,
            'date' => $date
        );

        $this->db->set($data);
        if ($this->db->insert('coil')) {
            return TRUE;
        }
        return FALSE;
    }

    function makeRegister($name, $pass, $permission)
    {

        $date = date('Y-m-d H:i:s', time());

        $data = array(
            'name' => $name,
            'password' => md5($pass),
            'real_pass' => $pass,
            'permission' => $permission,
            'date' => $date
        );

        $this->db->set($data);
        if ($this->db->insert('users')) {
            return TRUE;
        }
        return FALSE;
    }

    /*
    * Fix Row
    */

    function getAllCell()
    {
        $this->db->select('*')->from('cell');            
        $query = $this->db->get();

        if ($result = $query->result_array()) {
            return $result;
        }
        return false;
    }

    function updateFix($id, $row){
        
        $data = array(
            'weight' => $row
        );
        
        $this->db->where('cell_id', $id);
        if ($this->db->update('cell', $data)) {            
            return TRUE;
        }

        return FALSE;
    }

}