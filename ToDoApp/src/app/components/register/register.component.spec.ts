import { ComponentFixture, TestBed } from "@angular/core/testing";
import { TodoItemService } from "src/app/services/todo-item.service";
import { ToastrModule } from 'ngx-toastr';
import { FormGroup, FormsModule, NgForm } from "@angular/forms";
import { of, throwError } from "rxjs";
import { RegisterComponent } from "./register.component";
import { Register } from "src/app/models/register.model";

class MockToDoService {
    
    registerData: Register = new Register();
    registerUser() { 
        return of({ });
    }
}

describe('RegisterComponent', () => {
    let component: RegisterComponent;
    let fixture: ComponentFixture<RegisterComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [ToastrModule.forRoot(),
            FormsModule],
            declarations: [RegisterComponent],
            providers: [ 
                 { provide: TodoItemService, useClass: MockToDoService }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(RegisterComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('login', () => {
        it('should create new user when the registration is successful', () => {
            spyOn(component['toastr'], 'success');
            const formValue = {
                form: {
                    reset: () => null,
                } as unknown as FormGroup,
            };
            component.register(formValue as unknown as NgForm);
            expect(component.loading).toBeTrue();
            expect(component['toastr'].success).toHaveBeenCalledWith('Submitted successfully', 'User Register');
        });

        it('shouldnot create new user when the registration is unsuccessful', () => {
            spyOn(component['service'], 'registerUser').and.returnValue(throwError('user creation failed'));
            component.register({ form: {} as FormGroup } as NgForm);
            expect(component.loading).toBeFalse();
        });
    });
});