import { ComponentFixture, TestBed } from "@angular/core/testing";
import { TodoItemService } from "src/app/services/todo-item.service";
import { ToastrModule } from 'ngx-toastr';
import { of, throwError } from "rxjs";
import { Register } from "src/app/models/register.model";
import { TodoItemsComponent } from "./todo-items.component";
import { MatDialogModule } from "@angular/material/dialog";
import { TodoItem } from "src/app/models/todo-item.model";
import { FormsModule } from "@angular/forms";
import { CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";

class MockToDoService {
    list: TodoItem[]=[];
    registerData: Register = new Register();
    registerUser() { 
        return of({ });
    }
    putToDoItem() {
        return of({ });
    }

    refreshList() {
        this.list = [] as TodoItem[];
    }

    deleteToDoItem(id: number) {
        return of({});
    }
}

describe('TodoItemsComponent', () => {
    let component: TodoItemsComponent;
    let fixture: ComponentFixture<TodoItemsComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [ToastrModule.forRoot(), MatDialogModule, FormsModule],
            declarations: [TodoItemsComponent],
            schemas: [CUSTOM_ELEMENTS_SCHEMA],
            providers: [ 
                 { provide: TodoItemService, useClass: MockToDoService }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(TodoItemsComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('populateForm', () => {
        it('should open the dialog modal with the selected record', () => {
            spyOn(component['dialog'], 'open')
            const selectedItem = { itemDescription: 'test', id: 1, isCompleted: true } as TodoItem;
            component.populateForm(selectedItem);
            const response = component['service'].todoData;
            expect(response).not.toBeUndefined();
            expect(response.id).toEqual(1);
            expect(response.itemDescription).toEqual('test');
            expect(response.isCompleted).toBeTrue();
            expect(component['dialog'].open).toHaveBeenCalled();
        });
    });

    describe('addItem', () => {
        it('should open the empty dialog modal when add item button is clicked', () => {
            spyOn(component['dialog'], 'open')
            component.addItem();
            const response = component['service'].todoData;
            expect(response).not.toBeUndefined();
            expect(response.id).toEqual(0);
            expect(response.itemDescription).toBeUndefined();
            expect(response.isCompleted).toBeFalse();
            expect(component['dialog'].open).toHaveBeenCalled();
        });
    });

    describe('changeStatus', () => {
        it('should updated the status of the item in the backend successfully when checkbox is checked', () => {
            spyOn(component['toastr'], 'info');
            const selectedItem = { itemDescription: 'test', id: 1, isCompleted: true } as TodoItem;
            const statusValue = false;
            component.changeStatus(selectedItem, statusValue);
            const response = component['service'].todoData;
            expect(response).not.toBeUndefined();
            expect(response.id).toEqual(1);
            expect(response.itemDescription).toEqual('test');
            expect(response.isCompleted).toBeFalse();
            expect(component['toastr'].info).toHaveBeenCalledWith('Updated successfully', 'ToDo Item Updation');
        });

        it('should return error when the status of the item in the backend is not updated', () => {
            spyOn(component['service'], 'putToDoItem').and.returnValue(throwError('update failed'));
            spyOn(console, 'log');
            const selectedItem = { itemDescription: 'test', id: 1, isCompleted: true } as TodoItem;
            const statusValue = false;
            component.changeStatus(selectedItem, statusValue);
            expect(console.log).toHaveBeenCalledWith('update failed');
        });
    });

    describe('onDelete', () => {
        it('should delete the item in the backend successfully when deletion is confirmed', () => {
            spyOn(window, 'confirm').and.returnValue(true);
            spyOn(component['toastr'], 'error');
            component.onDelete(1);
            expect(component['toastr'].error).toHaveBeenCalledWith('Deleted successfully', 'ToDo Item Deletion');
        });

        it('should return error when the item in the backend is not deleted successfully', () => {
            spyOn(window, 'confirm').and.returnValue(true);
            spyOn(component['service'], 'deleteToDoItem').and.returnValue(throwError('deletion failed'));
            spyOn(console, 'log');
            component.onDelete(1);
            expect(console.log).toHaveBeenCalledWith('deletion failed');
        });
    });
});