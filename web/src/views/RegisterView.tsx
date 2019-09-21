import * as React from "react";
import SelectField from "../components/SelectField";
import TextField from "../components/TextField";
import Datepicker from "../components/Datepicker";
import * as M from "materialize-css";
import {config} from "../config";
import {Person} from "../api/model";

interface RegisterViewState {
  name: string
  surname: string
  age: number
  major: string
  gender: string
}

interface RegisterViewProps {
  onSuccess: (response: Person) => void
  onFailure: () => void
}

export class RegisterView<P extends RegisterViewProps, S extends RegisterViewState> extends React.Component<P, S> {
  constructor(props: P) {
    super(props)
    this.state = {
      age: 0,
      gender: "",
      major: "",
      surname: "",
      name: ""
    } as S
  }

  render() {
    let form = this
    return (
      <form className="col offset-l4 l4 offset-s1 s10" onSubmit={event => {
        event.preventDefault()
        console.log("test")
        fetch(config.apiUrl + "/person", {
          method: "post",
          body: JSON.stringify(this.state),
        }).then(response => {
          console.log("got response")
          if (response.status !== 201) {
            throw Error(`invalid status: ${response.status} ${response.statusText}`)
          }
          return response.json()
        }).then(body => {
          console.log(body)
          this.props.onSuccess(body as Person)
        }).catch(reason => {
          console.log(reason)
          this.props.onFailure()
        })
      }}>
        <div className="row">
          <SelectField name={"Ansprache"} className="l3 s12"
                       options={new Map<string, string>([
                         ["d", "-"],
                         ["m", "Herr"],
                         ["f", "Frau"]
                       ])}
                       onChange={value => this.setState({gender: value})}/>
          <TextField name="Name" className="l4 s6"
                     onChange={value => this.setState({name: value})}/>
          <TextField name="Nachname" className="l5 s6"
                     onChange={value => this.setState({surname: value})}/>
        </div>
        <div className="row">
          <SelectField name={"Studiengang"} placeholder={"Waehle deinen Studiengang"} className="l6 s12"
                       options={new Map<string, string>([
                         ["ET", "Elektrotechnik"],
                         ["INF", "Informatik"],
                         ["MCD", "Media & Communication for Digital Business"],
                         ["WI", "Wirtschaftsinformatik"]
                       ])}
                       onChange={value => this.setState({major: value})}/>
          <Datepicker name="Geburtstag" className="l6 s12" cssOptions={{
            onClose: function (this: M.Datepicker) {
              let value = this.date
              if (!value) return
              let years = (new Date()).getFullYear() - value.getFullYear()
              value.setFullYear((new Date()).getFullYear())
              if (new Date() < value) {
                years -= 1
              }
              form.setState({age: years})
            },
            maxDate: new Date(2005, 11, 31),
            defaultDate: new Date(2005, 11, 31),
            showDaysInNextAndPreviousMonths: true,
            i18n: {
              cancel: "Abbrechen",
              months: [
                "Januar",
                "Februar",
                "Maerz",
                "April",
                "Mai",
                "Juni",
                "Juli",
                "August",
                "September",
                "Oktober",
                "November",
                "Dezember"
              ],
              monthsShort: ["Jan", "Feb", "Mar", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez"],
              weekdaysShort: ["So", "Mo", "Di", "Mi", "Do", "Fr", "Sa"],
              weekdaysAbbrev: ["S", "M", "D", "M", "D", "F", "S"]
            },
            format: "dd.mm.yyyy"
          }}/>
        </div>
        <div className="row">
          <div className="col offset-xl8 xl4 offset-l6 l6 offset-m4 m8 offset-s4 s8">
            <button className="waves-effect waves-light btn" type="submit">
              <i className="material-icons right">send</i>Abschicken
            </button>
          </div>
        </div>
      </form>
    )
  }
}
