import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Register } from '../../models/register.model';
import { TodoItemService } from '../../services/todo-item.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styles: [
  ]
})
export class RegisterComponent {
  loading = false;

  constructor(public service: TodoItemService,
    private toastr: ToastrService) { }

  register(form: NgForm) {
    this.loading = true;

    this.service.registerUser().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.success('Submitted successfully', 'User Register');
      },
      err => {
        this.loading = false;
      }
    );
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.service.registerData = new Register();
  }
}
