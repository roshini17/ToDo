import {Component } from '@angular/core';
import { MatDialogRef } from "@angular/material/dialog";
import { NgForm} from "@angular/forms";
import { TodoItemService } from 'src/app/services/todo-item.service';
import { ToastrService } from 'ngx-toastr';
import { TodoItemsComponent } from '../todo-items/todo-items.component';
import { TodoItem } from 'src/app/models/todo-item.model';

@Component({
    selector: 'app-todo-item-modal',
    templateUrl: './todo-items-modal.component.html',
    styleUrls: ['./todo-items-modal.component.scss']
})
export class TodoItemsModalComponent {

  constructor(
    public dialogRef: MatDialogRef<TodoItemsComponent>,
    public service: TodoItemService,
    private toastr: ToastrService) {
  }

  onSubmit(form: NgForm) {
    if (this.service.todoData.id == 0) {
      this.addRecord(form);
    } else {
      this.updateRecord(form);
    }
    this.resetForm(form);
    this.dialogRef.close(form.value);
  }

  addRecord(form: NgForm) {
    this.service.postToDoItem().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
        this.toastr.success('Created successfully', 'ToDo Item Creation');
      },
      err => { console.log(err); }
    );
  }

  updateRecord(form: NgForm) {
    this.service.putToDoItem().subscribe(
      res => {
        this.resetForm(form);
        this.service.refreshList();
        this.toastr.info('Updated successfully', 'ToDo Item Updation');
      },
      err => { console.log(err); }
    );
  }

  resetForm(form: NgForm) {
    form.form.reset();
    this.service.todoData = new TodoItem();
  }
}