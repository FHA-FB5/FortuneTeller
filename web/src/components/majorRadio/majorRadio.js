import React, { Component } from 'react' 
import './majorRadio.css'

class MajorRadio extends Component {
  
  render() {
    let cn = "majorRadio-wrapper"
    
    switch (this.props.valid) {
      case true:
        cn += " valid"
        break;
      case false:
        cn += " invalid"
        break;
      default:
    }
    
    return (
      <div className={ cn }>
        <label className="majorRadio-label">{ this.props.label }</label>
        <div className="majorRadio-inner">
            <div>
              <input name="major" className="majorRadio-input" type="radio" value="inf" onChange={ this.props.onChange } />
              <label className="majorRadio-inner-label">INF</label>
            </div>
            <div>
              <input name="major" className="majorRadio-input" type="radio" value="et" onChange={ this.props.onChange } />
              <label className="majorRadio-inner-label">ET</label>
            </div>
            <div>
              <input name="major" className="majorRadio-input" type="radio" value="mcd" onChange={ this.props.onChange } />
              <label className="majorRadio-inner-label">MCD</label>
            </div>
            <div>
              <input name="major" className="majorRadio-input" type="radio" value="wi" onChange={ this.props.onChange } />
              <label className="majorRadio-inner-label">WI</label>
            </div>
        </div>
      </div>      
    )
  }
    
}

export default MajorRadio