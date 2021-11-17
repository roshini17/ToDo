import { ComponentFixture, TestBed } from "@angular/core/testing";
import { TodoItemService } from "src/app/services/todo-item.service";
import { ToastrModule } from 'ngx-toastr';
import { of, throwError } from "rxjs";
import { Register } from "src/app/models/register.model";
import { MatDialogRef } from "@angular/material/dialog";
import { TodoItem } from "src/app/models/todo-item.model";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { TodoItemsModalComponent } from "./todo-items-modal.component";
import { FormGroup, FormsModule, NgForm } from "@angular/forms";

class MatDialogRefMock {
    close(value = '') {
        close();
    }
}

class MockToDoService {
    list: TodoItem[]=[];
    todoData: TodoItem = new TodoItem();
    registerData: Register = new Register();
    registerUser() { 
        return of({ });
    }
    putToDoItem() {
        return of({ });
    }

    postToDoItem() {
        return of({ });
    }

    refreshList() {
        this.list = [] as TodoItem[];
    }

    deleteToDoItem(id: number) {
        return of({});
    }
}

describe('TodoItemsModalComponent', () => {
    let component: TodoItemsModalComponent;
    let fixture: ComponentFixture<TodoItemsModalComponent>;
    const formValue = {
        form: {
            reset: () => null,
        } as unknown as FormGroup,
    };

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [ToastrModule.forRoot(), FormsModule],
            declarations: [TodoItemsModalComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [ 
                { provide: MatDialogRef, useClass: MatDialogRefMock },
                { provide: TodoItemService, useClass: MockToDoService }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(TodoItemsModalComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('onSubmit', () => {
        it('should create new record and save to db when id is empty', () => {
            spyOn(component['toastr'], 'success');
            component.onSubmit(formValue as unknown as NgForm);
            expect(component['toastr'].success).toHaveBeenCalledWith('Created successfully', 'ToDo Item Creation');
        });

        it('should create new record and save to db when id is not empty', () => {
            component['service'].todoData = { id: 1, itemDescription: 'test', isCompleted: false } as TodoItem;
            spyOn(component['toastr'], 'info');
            component.onSubmit(formValue as unknown as NgForm);
            expect(component['toastr'].info).toHaveBeenCalledWith('Updated successfully', 'ToDo Item Updation');
        });
    });

    describe('addRecord', () => {
        it('should return error when the item creation in the backend is not unsuccessful', () => {
            spyOn(component['service'], 'postToDoItem').and.returnValue(throwError('creation failed'));
            spyOn(console, 'log');
            component.addRecord({ form: {} as FormGroup } as NgForm);
            expect(console.log).toHaveBeenCalledWith('creation failed');
        });
    });

    describe('updateRecord', () => {
        it('should return error when the item updation in the backend is not unsuccessful', () => {
            spyOn(component['service'], 'putToDoItem').and.returnValue(throwError('update failed'));
            spyOn(console, 'log');
            component.updateRecord({ form: {} as FormGroup } as NgForm);
            expect(console.log).toHaveBeenCalledWith('update failed');
        });
    });
});
