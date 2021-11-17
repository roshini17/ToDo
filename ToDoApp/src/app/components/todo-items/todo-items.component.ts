import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { TodoItem } from '../../models/todo-item.model';
import { TodoItemService } from '../../services/todo-item.service';
import { MatDialog } from '@angular/material/dialog';
import { TodoItemsModalComponent } from '../todo-items-modal/todo-items-modal.component';

@Component({
  selector: 'app-todo-items',
  templateUrl: './todo-items.component.html',
  styleUrls: ['./todo-items.component.scss']
})
export class TodoItemsComponent implements OnInit {


  constructor(public service: TodoItemService,
    private toastr: ToastrService,
    private dialog: MatDialog) { }
    
  ngOnInit(): void {
    this.service.refreshList();
  }

  populateForm(selectedRecord: TodoItem) {
    this.service.todoData = Object.assign({}, selectedRecord);
    this.dialog.open(TodoItemsModalComponent, {
      width: '600px',
      height: '400px'
    });
  }

  addItem() {
    this.dialog.open(TodoItemsModalComponent, {
      width: '600px',
      height: '400px'
    });
    this.service.todoData = new TodoItem();
  }

  changeStatus(selectedRecord: TodoItem, value: boolean ) {
    selectedRecord.isCompleted = value;
    this.service.todoData = Object.assign({}, selectedRecord);
    this.service.putToDoItem().subscribe(
      res => {
        this.service.refreshList();
        this.toastr.info('Updated successfully', 'ToDo Item Updation');
      },
      err => { console.log(err); }
    );
  }

  onDelete(id: number) {
    if (confirm('Are you sure you want to delete this record?')) {
      this.service.deleteToDoItem(id)
      .subscribe( res => {
        this.service.refreshList();
        this.toastr.error('Deleted successfully', 'ToDo Item Deletion');
      },
      err => {
        console.log(err);
      })
    }
  }
}
