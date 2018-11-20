import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import 'rxjs/add/operator/map';
import {Account} from "../../model/account.interface";
import {SearchCell} from "../../model/serach-cell.interface";


@Injectable()
export class DataService {

  private url: string = 'http://rpcksrv/WarehouseService/index.php/Service/';
  // private url: string = 'http://dvlpr/WarehouseService/index.php/Service/';
  // private url: string = 'http://localhost:8888/WarehouseService/Service/';
  constructor(private httpClient: HttpClient) {
  }

  login(name: string, pass: string) {

    const data = JSON.stringify({
      'name' : name,
      'password' : pass
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<Account>(`${this.url}login`, data, {headers});

  }

  register(name, password, permission){
    const data = JSON.stringify({
      'name' : name,
      'password' : password,
      'permission' : permission
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<Account>(`${this.url}register`, data, {headers});
  }

  insertCoilAndCell(coil, cell, position, id){
    const data = JSON.stringify({
      'coil' : coil,
      'cell' : cell,
      'position' : position,
      'user_id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}insertCoilAndCell`, data, {headers});
  }

  insertInventoryCoilAndCell(coil, cell, position, id){
    const data = JSON.stringify({
      'coil' : coil,
      'cell' : cell,
      'position' : position,
      'user_id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}insertInventoryCoilAndCell`, data, {headers});
  }
  // recordCoilCell(cellId: string, coil: string, cell: string, userId: string){
  //   const data = JSON.stringify({
  //     'coil' : coil,
  //     'cell_id' : cellId,
  //     'cell' : cell,
  //     'user_id' : userId
  //   });
  //
  //   let headers = new HttpHeaders();
  //   headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');
  //
  //   return this.httpClient.post<Account>(`${this.url}makeRecord`, data, {headers});
  //
  // }

  searchEmptyCell(coil) {
    const data = JSON.stringify({
      'coil' : coil
    });
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}searchEmptyCell`, data, {headers});

  }

  getVersion() {
    return this.httpClient.get<any>(`${this.url}getVersion`);
  }
  getAllCoilInCell(){
      return this.httpClient.get<any>(`${this.url}getAllCoilInCell`);
  }

  getAllZiroCoilInCell(){
    return this.httpClient.get<any>(`${this.url}getAllZiroCoilInCell`);
  }


  getAllCoilInCellActiveAndUnactive(){
    return this.httpClient.get<any>(`${this.url}getAllCoilInCellActiveAndUnactive`);
  }

  countStatistic() {
    return this.httpClient.get<SearchCell>(`${this.url}countStatistic`);
  }

  rowStatistic() {
    return this.httpClient.get<SearchCell>(`${this.url}capacityByRow`);
  }

  moveInOneCell(id: string){
    const data = JSON.stringify({
      'id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<Account>(`${this.url}moveInOneCell`, data, {headers});
  }
  rowProStatistic() {
    return this.httpClient.get<SearchCell>(`${this.url}capacityByProRow`);
  }

  allUsers(id: string){
    const data = JSON.stringify({
      'id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<Account>(`${this.url}allUser`, data, {headers});
  }

  getCellWeight(cell:string){
    const data = JSON.stringify({
      'cell' : cell
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}getCellWeight`, data, {headers});
  }

  deleteUser(id: string, name: string){
    const data = JSON.stringify({
      'id' : id,
      'name' : name
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<Account>(`${this.url}deleteUser`, data, {headers});
  }

  changeWeight(id, weight){
    const data = JSON.stringify({
      'id':id,
      'weight':weight
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}changeWeight`, data, {headers});
  }

  changeCapacity(id, capacity){
    const data = JSON.stringify({
      'id':id,
      'capacity':capacity
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}changeCapacity`, data, {headers});
  }

  changePriority(id, priority){
    const data = JSON.stringify({
      'id':id,
      'priority':priority
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}changePriority`, data, {headers});
  }

  searchCoilInStorage(coil: string){

    const data = JSON.stringify({
      'coil' : coil
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<SearchCell>(`${this.url}searchCoil`, data, {headers});
  }

  syncWareHouse(id: string){

    const data = JSON.stringify({
      'user_id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}inventorySync`, data, {headers});
  }

  syncCheckWareHouse(id: string){

    const data = JSON.stringify({
      'user_id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}checkSync`, data, {headers});
  }

  takeCoil(cell_id: string, coil_id: string, cell: string, coil: string, position:string){
    const data = JSON.stringify({
      'cell_id' : cell_id,
      'coil_id' : coil_id,
      'cell' : cell,
      'coil' : coil,
      'position' : position
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<SearchCell>(`${this.url}takeCoil`, data, {headers});
  }

  checkCoilCell(coil: string, cell: string){

    const data = JSON.stringify({
      'cell' : cell,
      'coil' : coil
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<any>(`${this.url}checkCoilCell`, data, {headers});

  }

  newPosition(coil: string, cell: string, id: string, version: string){
    const data = JSON.stringify({
      'cell' : cell,
      'coil' : coil,
      'version' : version,
      'user_id' : id
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<SearchCell>(`${this.url}recordNewPosition`, data, {headers});
  }

  overWriteRecord(coil: string, cell: string){
    const data = JSON.stringify({
      'cell' : cell,
      'coil' : coil
    });

    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

    return this.httpClient.post<SearchCell>(`${this.url}overWriteRecord`, data, {headers});

  }

  getChanges(){
    return this.httpClient.get<SearchCell>(`${this.url}getChanges`);
  }
}
