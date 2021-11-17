import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Login } from '../../models/login.model';
import { TodoItemService } from '../../services/todo-item.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: [
  ]
})
export class LoginComponent {
  invalidLogin: boolean | undefined;
  loading = false;

  constructor(public service: TodoItemService,
    private toastr: ToastrService, private router: Router) { }

  login(form: NgForm) {
    this.loading = true;
    this.service.loginUser().subscribe(
      res => {
        const token = (<any>res).token;
        localStorage.setItem('jwt', token); 
        this.invalidLogin = false;
        this.resetForm(form)
        this.router.navigate(['/']);
        this.toastr.success('LoggedIn successfully', 'User Login');
      },
      err => {
        this.invalidLogin = true;
        this.loading = false;
      }
    );
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.service.loginData = new Login();
  }
}
