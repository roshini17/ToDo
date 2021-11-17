import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TodoItem } from '../models/todo-item.model';
import { Login } from '../models/login.model';
import { Register } from '../models/register.model';

@Injectable({
  providedIn: 'root'
})
export class TodoItemService {
  readonly baseURL = 'https://localhost:44393/api/ToDoItems';
  readonly authURL = 'https://localhost:44393/api/Authentication';
  list: TodoItem[]=[];
  token: any = localStorage.getItem('jwt');
  headerDetails: HttpHeaders = new HttpHeaders({
    'Content-Type': 'application/json'
  });

  constructor(private http: HttpClient) { }

  todoData: TodoItem = new TodoItem();
  loginData: Login = new Login();
  registerData: Register = new Register();

  postToDoItem() {
    return this.http.post(this.baseURL, this.todoData, { headers: this.headerDetails });
  }

  putToDoItem() {
    return this.http.put(`${this.baseURL}/${this.todoData.id}`, this.todoData, { headers: this.headerDetails });
  }

  deleteToDoItem(id: number) {
    return this.http.delete(`${this.baseURL}/${id}`, { headers: this.headerDetails });
  }

  refreshList() {
    this.http.get(this.baseURL, { headers: this.headerDetails })
    .toPromise()
    .then(res => {
      this.list = res as TodoItem[]
    });
  }

  loginUser() {
    return this.http.post(`${this.authURL}/login`, this.loginData, { headers: this.headerDetails });
  }

  registerUser() {
    return this.http.post(`${this.authURL}/register`, this.registerData, { headers: this.headerDetails });
  }
}
