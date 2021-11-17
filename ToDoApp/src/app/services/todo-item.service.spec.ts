import { HttpClientModule } from "@angular/common/http";
import { TestBed } from "@angular/core/testing";
import { TodoItemService } from "./todo-item.service";

describe('TodoItemService', () => {
    let service: TodoItemService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientModule],
        });
        service = TestBed.inject(TodoItemService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });

    describe('postToDoItem', () => {
        it('should make an api call to create toDoItem', () => {
            spyOn(service['http'], 'post');
            service.postToDoItem();
            expect(service['http'].post).toHaveBeenCalled();
        });
    });

    describe('putToDoItem', () => {
        it('should make an api call to update toDoItem', () => {
            spyOn(service['http'], 'put');
            service.putToDoItem();
            expect(service['http'].put).toHaveBeenCalled();
        });
    });

    describe('deleteToDoItem', () => {
        it('should make an api call to delete toDoItem', () => {
            spyOn(service['http'], 'delete');
            service.deleteToDoItem(1);
            expect(service['http'].delete).toHaveBeenCalled();
        });
    });

    describe('loginUser', () => {
        it('should make an api call to authenticate user', () => {
            spyOn(service['http'], 'post');
            service.loginUser();
            expect(service['http'].post).toHaveBeenCalled();
        });
    });

    describe('registerUser', () => {
        it('should make an api call to register user', () => {
            spyOn(service['http'], 'post');
            service.registerUser();
            expect(service['http'].post).toHaveBeenCalled();
        });
    });
});