import { Component, Input } from '@angular/core';
import { IResult } from '../../Interfaces/result'
import { QuizService } from '../../services/quiz.service'

@Component({
    selector: 'app-result-list',
    template: `<h3>Results</h3>
<div *ngIf="results && results.length>0">
    <table class="table table-striped table-hover">
        <tr>
            <th>Text</th>
            <th>Min Value</th>
            <th>Max Value</th>
            <th></th>
        </tr>
        <tr *ngFor="let r of results">
            <td>{{r.text}}</td>
            <td>{{r.minValue  || 'N/A'}}</td>
            <td>{{r.maxValue  || 'N/A'}}</td>
            <td>
                <a [routerLink]="['/quiz', quizId, 'results', r.id]" class="btn btn-info">Edit</a>
                <a (click)="removeResult(r)" class="btn btn-danger ">Delete</a>
            </td>
        </tr>
    </table>
</div>
<a [routerLink]="['/quiz', quizId, 'results', 0 ]" class="btn btn-warning">Add a new Result</a>
`,
    styles: [``]
})
export class ResultListComponent {
    @Input() quizId: number | undefined;
    results: IResult[] | undefined;

    constructor(private quizService: QuizService) { }

    ngOnInit() {
        this.loadResults();
    }

    removeResult(result: IResult) {
        if (confirm('are you sure to delete?')) {
            this.quizService.removeResult(result.quizId, result.id).subscribe(
                _ => {
                    this.results = this.results!.filter(q => q !== result);
                },
                error => console.log(error)
            );
        }
    }

    private loadResults() {
        this.quizService.getResults(this.quizId!)
            .subscribe(data => this.results = data);
    }
}