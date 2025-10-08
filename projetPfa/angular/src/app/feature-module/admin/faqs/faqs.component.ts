import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Sort } from '@angular/material/sort';
import { ToastrService } from 'ngx-toastr';
import { Faq } from 'src/app/core/models/Site/Faq'
import { FaqsService } from 'src/app/core/service/Backend/SIte/faqs.service';
@Component({
  selector: 'app-faqs',
  templateUrl: './faqs.component.html',
  styleUrl: './faqs.component.scss'
})
export class FAQsComponent {
  public faqsList: Faq[]=[];
  faqForm: FormGroup;

  constructor(private faqService: FaqsService,private toastr: ToastrService,private fb: FormBuilder) 
  {
    this.faqForm = this.fb.group({
      question: ['', Validators.required],
      response: ['', Validators.required]
    });
   }
  
  ngOnInit(): void {
    this.loadFaqs();
  }

  isLoading = false;
  errorMessage= '';
  loadFaqs(): void {
    this.isLoading = true;
    this.faqService.getFaqs().subscribe({
      next: (faqs) => {
        this.faqsList = faqs;
        this.isLoading = false;
        this.errorMessage= '';
      },
      error: (err) => {
        this.errorMessage = 'Failed to load Faqs';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  public sortData(event: Sort) {
        const data = this.faqsList.slice();
    
        if (!event.active || event.direction === '') {
          this.faqsList = data;
        } else {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          this.faqsList = data.sort((a: any, b: any) => {
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            const aValue = (a as any)[event.active];
            // eslint-disable-next-line @typescript-eslint/no-explicit-any
            const bValue = (b as any)[event.active];
            return (aValue < bValue ? -1 : 1) * (event.direction === 'asc' ? 1 : -1);
          });
        }
    }

    public deletingFaq:any;
    setSelectedFaq(faq: Faq): void {
      this.deletingFaq = faq;
    }

    deleteFaq(id: number): void {
      this.faqService.deleteFaq(id).subscribe({
        next: (response) => {
          // Remove the deleted FAQ from the list
          this.faqsList = this.faqsList.filter(faq => faq.id !== id);
          this.toastr.success('Success', 'FAQ deleted successfully');
        },
        error: (err) => {
          this.toastr.error('Error' , 'Delete failed');
        }
      });
    }
    


  public newFaq = { question: '', response: '' };  
  public showForm = false;  

  toggleForm() {
    this.showForm = !this.showForm;  // Toggle the form visibility
  }

  
  addFaq() {
    this.newFaq.question=this.faqForm.controls['question'].value;
    this.newFaq.response=this.faqForm.controls['response'].value;
    console.log("DSASDf",this.newFaq);
    if (this.newFaq.question && this.newFaq.response) {
      this.faqService.createFaq(this.newFaq).subscribe({
        next: (response) => {
          this.faqsList.push(response);  // Add new FAQ to the list
          this.newFaq = { question: '', response: '' };  // Reset the form
          this.showForm = false;  // Hide the form after submitting
          this.toastr.success('Success', 'FAQ has been added');
        },
        error: (err) => {
          console.log("hahah ",err.error);
          if (err.status === 400) {
            // Handle validation errors from backend
            this.toastr.error('Validation Error: ' , JSON.stringify(err.error.errors));
          } else {
            // Handle general errors
            this.toastr.error('Error ', 'Failed to add faq');
          }
        }
      });
    }
  }
  
}
