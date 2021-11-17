import { ComponentFixture, TestBed } from "@angular/core/testing";
import { TodoItemService } from "src/app/services/todo-item.service";
import { LoginComponent } from "./login.component";
import { ToastrModule } from 'ngx-toastr';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from "@angular/router/testing";
import { FormGroup, FormsModule, NgForm } from "@angular/forms";
import { Login } from "src/app/models/login.model";
import { of, throwError } from "rxjs";

class MockToDoService {
    
    loginData: Login = new Login();
    loginUser() { 
        return of({
            token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJ0b2RvYXBpIiwibmJmIjoxNDk4MTE3NjQyLCJleHAiOjE0OTgxMjEyNDIsInVpZCI6MSwicm9sZSI6ImFkbWluIn0.ZDz_1vcIlnZz64nSM28yA1s-4c_iw3Z2ZtP-SgcYRPQ'
        });
    }

}

describe('LoginComponent', () => {
    let component: LoginComponent;
    let fixture: ComponentFixture<LoginComponent>;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule, ToastrModule.forRoot(), RouterTestingModule,
            FormsModule],
            declarations: [LoginComponent],
            providers: [ 
                 { provide: TodoItemService, useClass: MockToDoService }
            ]
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(LoginComponent);
        component = fixture.componentInstance;

        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('login', () => {
        it('should navigate to the dashboard when the authentication is successful', () => {
            spyOn(component['router'], 'navigate');
            spyOn(component['toastr'], 'success');
            const formValue = {
                form: {
                    reset: () => null,
                } as unknown as FormGroup,
            };
            component.login(formValue as unknown as NgForm);
            expect(component.loading).toBeTrue();
            expect(component.invalidLogin).toBeFalse();
            expect(component['router'].navigate).toHaveBeenCalledWith(['/']);
            expect(component['toastr'].success).toHaveBeenCalledWith('LoggedIn successfully', 'User Login');
        });

        it('shouldnot navigate to dashboard when the authentication is unsuccessful', () => {
            spyOn(component['service'], 'loginUser').and.returnValue(throwError('login failed'));
            component.login({ form: {} as FormGroup } as NgForm);
            expect(component.loading).toBeFalse();
            expect(component.invalidLogin).toBeTrue();
        });
    });
});