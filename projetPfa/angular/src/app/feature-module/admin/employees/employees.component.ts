import { Component } from '@angular/core';
import { routes } from 'src/app/core/core.index';
import { employee } from 'src/app/core/models/employee/employee';
import { EmployeesService } from 'src/app/core/service/Backend/employees/employees.service';
import { MatDialog } from '@angular/material/dialog';
import { UpdateEmployeeComponent } from './update-employee/update-employee.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';
import { MatTableDataSource } from '@angular/material/table';
import { AddEmployee } from 'src/app/core/models/employee/addEmployee';
import { ToastrService } from 'ngx-toastr';
interface data {
  value: string;
}
interface EmployeeFormData {
  prenom: string;
  nom: string;
  email: string;
  phoneNumber: string;
  salaire: number;
  birthday: string; 
  imageUrl?: string; // Add this line
}

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.scss'
})
export class EmployeesComponent {
  public routes = routes;
  public selectedValue1 = '';
  isClassAdded: boolean[] = [false, false, false];
  selectedList1: data[] = [
    {value: 'Relevance'},
    {value: 'Price'}
  ];
  employees: employee[] = [];
  employeesStack: employee[] = [];
  isLoading = false;
  errorMessage = '';

  public searchDataValue = '';
  dataSource!: MatTableDataSource<employee>;

  constructor(private employeeService: EmployeesService, private dialog: MatDialog, private toastr: ToastrService) {
  }


  toggleClass(index: number) {
    this.isClassAdded[index] = !this.isClassAdded[index];
  }

  ngOnInit(): void {
    this.loadUsers();
  }
 //list employee
 public searchData(value: string): void {
  const searchTerm = value.trim().toLowerCase();
  
  if (!searchTerm) {
    return;
  }

  this.employees = this.employeesStack.filter(employee => {
    return (
      (employee.username?.toLowerCase().includes(searchTerm)) ||
      (employee.nom?.toLowerCase().includes(searchTerm)) ||
      (employee.prenom?.toLowerCase().includes(searchTerm))
    );
  });
}

  loadUsers(): void {
    this.isLoading = true;
    this.employeeService.getEmployees().subscribe({
      next: (users) => {
        this.employees = users;
        this.employeesStack = users;
        this.isLoading = false;
        console.error(users);
      },
      error: (err) => {
        this.errorMessage = 'Failed to load users';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  private refreshEmployeeList(): void {
    // Option 1: If you're using a simple array
    this.employeeService.getEmployees().subscribe({
      next: (employees) => {
        this.employees = employees;
        this.employeesStack = employees;// Replace the entire list
      },
      error: (err) => {
        console.error('Failed to refresh employee list:', err);
        this.toastr.error('Error', 'Failed to refresh employee data');
      }
    });
  }

  calculateAge(birthDate: string | Date): number {
    const dob = new Date(birthDate);
    const diff = Date.now() - dob.getTime();
    const ageDate = new Date(diff); 
    return Math.abs(ageDate.getUTCFullYear() - 1970);
  }
//end list 
//delete employee
selectedEmployee: any;

  deleteEmployee(id:number):void {
    this.employeeService.deleteEmployee(id).subscribe({
      next: (response) => {
        this.toastr.success('Success', 'Employee has been deleted');
        this.refreshEmployeeList();
      },
      error: (err) => {
        this.toastr.error('Error', 'deleting Employee failed');
      }
    })
  }
  
  setSelectedEmployee(employee: any) {
    this.selectedEmployee = employee;
  }
//end delete employee
//update employee
  openUpdateDialog(employee: any): void {
    const dialogRef = this.dialog.open(UpdateEmployeeComponent, {
      width: '800px',
      disableClose: true, // Prevent closing by clicking outside
      data: { employee }
    });
  
    // Store the subscription so we can unsubscribe later
    const sub = dialogRef.componentInstance.onSubmitSuccess.subscribe((updatedData) => {
      const empUpdate: employee = this.mapFormDataToEmployee(updatedData, employee);
      this.employeeService.updateEmployee(empUpdate).subscribe({
        next: (response) => {
          dialogRef.close();
          this.refreshEmployeeList();
          this.toastr.success('Success', 'Employee updated successfully');
        },
        error: (err) => {
          // Handle API validation errors
          if (err.status === 400 && err.error.errors) {
            dialogRef.componentInstance.setErrors(err.error.errors);
          } else {
            this.toastr.error('Error', 'Update failed');
          }
        }
      });
    });
  
    // Clean up subscription when dialog closes
    dialogRef.afterClosed().subscribe(() => {
      sub.unsubscribe();
    });
  }

  private mapFormDataToEmployee(formData: EmployeeFormData, originalEmployee: employee): employee {
    const date = new Date(formData.birthday);
    const isoString = date.toISOString();
    
    return {
      ...originalEmployee, 
      prenom: formData.prenom,
      nom: formData.nom,
      email: formData.email,
      phoneNumber: formData.phoneNumber,
      salaire: formData.salaire,
      birthday: isoString,
      imageUrl: formData.imageUrl || originalEmployee.imageUrl // Add this line
    };
  }
//end update employee 
//add employee  
// ... existing imports ...

openAddEmployeeDialog(): void {
  const dialogRef = this.dialog.open(AddEmployeeComponent, {
    width: '800px',
    disableClose: true,
  });

  const sub = dialogRef.componentInstance.onSubmitSuccess.subscribe((employeeData: AddEmployee) => {
    this.employeeService.createEmployee(employeeData).subscribe({
      next: (response) => {
        dialogRef.close();
        this.refreshEmployeeList();
        this.toastr.success('Success', 'Employee added successfully');
      },
      error: (err) => {
        dialogRef.componentInstance.isSubmitting = false;
        if (err.status === 400 && err.error.errors) {
          // Pass validation errors to the form
          dialogRef.componentInstance.setErrors(err.error.errors);
        } else {
          // Show other errors in snackbar
          this.toastr.error('Error', 'Failed to add employee');
        }
      }
    });
  });

  dialogRef.afterClosed().subscribe(() => {
    sub.unsubscribe();
  });
}
  

}
